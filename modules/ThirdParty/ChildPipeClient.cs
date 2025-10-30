using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ThirdParty
{
    internal static class Framing
    {
        public static void ReadExactly(Stream s, Span<byte> buf)
        {
            int read = 0;
            while (read < buf.Length)
            {
                int n = s.Read(buf[read..]);
                if (n == 0) throw new EndOfStreamException("EOF");
                read += n;
            }
        }

        public static void WriteFrame(Stream s, ReadOnlySpan<byte> payload)
        {
            Span<byte> hdr = stackalloc byte[4];
            BinaryPrimitives.WriteInt32BigEndian(hdr, payload.Length);
            s.Write(hdr);
            s.Write(payload);
            s.Flush();
        }

        public static byte[] ReadFrame(Stream s)
        {
            Span<byte> hdr = stackalloc byte[4];
            ReadExactly(s, hdr);
            int len = BinaryPrimitives.ReadInt32BigEndian(hdr);
            if (len <= 0) throw new InvalidDataException($"Invalid length {len}");
            var buf = new byte[len];
            ReadExactly(s, buf);
            return buf;
        }
    }
    internal class ChildPipeClient : IDisposable
    {
        private readonly Process _child;
        private readonly NamedPipeClientStream _pipe;

        public ChildPipeClient(string pythonExe, string childScriptPath)
        {
            // 파이프 이름은 충돌 방지를 위해 GUID 사용
            string pipeName = "myapp_rpc_" + Guid.NewGuid().ToString("N");

            var psi = new ProcessStartInfo
            {
                //FileName = "E:\\GitHub\\capstone\\thirdparty\\dist\\MyOCRApp\\MyOCRApp.exe",                         // 예: "python" 또는 전체 경로
                //Arguments = $"{pipeName}", 
                FileName = pythonExe,
                Arguments = $"{childScriptPath} {pipeName}",
                UseShellExecute = false,
                RedirectStandardError = true,
                StandardErrorEncoding = Encoding.UTF8,
                CreateNoWindow = false
            };
            psi.Environment["PARENT_PID"] = Environment.ProcessId.ToString(); 

            _child = Process.Start(psi) ?? throw new InvalidOperationException("Failed to start child");
            _child.ErrorDataReceived += (_, e) => { if (e.Data != null) Debug.WriteLine("[child] " + e.Data); };
            _child.BeginErrorReadLine();

            Debug.WriteLine(pipeName);

            _pipe = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.None);
            _pipe.Connect(600000); // 최대 5초 대기
        }

        public JsonDocument Call(string method, object? body)
        {
            var req = new { method, body };
            var payload = JsonSerializer.SerializeToUtf8Bytes(req);
            Framing.WriteFrame(_pipe, payload);

            var respBytes = Framing.ReadFrame(_pipe);
            return JsonDocument.Parse(respBytes);
        }

        public void Dispose()
        {
            try { _pipe?.Dispose(); } catch { }
            try
            {
                if (!_child.HasExited) _child.Kill(true);
                _child.Dispose();
            }
            catch { }
        }

        static string Quote(string s) => s.Contains(' ') ? $"\"{s}\"" : s;
    }
}
