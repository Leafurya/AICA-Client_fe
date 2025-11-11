using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Pronunciation
{
    internal class AudioStreamer : IDisposable
    {
        public const int ChunkSamples = 4096;
        private const int BitsPerSample = 16;
        private const int Channels = 1;


        private WaveInEvent? _waveIn;
        private MemoryStream? _buffer;

        public string? GetDeviceName()
        {
            if (_waveIn == null)
                return null;

            try
            {
                int index = _waveIn.DeviceNumber;
                var info = WaveIn.GetCapabilities(index);

                // MMDeviceEnumerator로 전체 장치 목록 가져오기
                var enumerator = new MMDeviceEnumerator();
                var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

                // ProductName 앞부분이 일치하는 장치를 찾아서 FriendlyName 반환
                var match = devices.FirstOrDefault(d =>
                    d.FriendlyName.StartsWith(info.ProductName, StringComparison.OrdinalIgnoreCase));

                // 매칭된 장치가 있으면 FriendlyName, 없으면 기존 ProductName 반환
                return match?.FriendlyName ?? info.ProductName;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetDeviceName error: " + ex.Message);
                return null;
            }
        }
        public int GetDeviceSampleRateOrDefault(int fallback)
        {
            try
            {
                using var temp = new WaveInEvent();
                return temp.WaveFormat.SampleRate;
            }
            catch { return fallback; }
        }
        public async Task StartAsync(int sampleRate, Func<byte[], Task> onChunk, CancellationToken ct)
        {
            _buffer = new MemoryStream();


            _waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(sampleRate, BitsPerSample, Channels),
                BufferMilliseconds = 20,
                NumberOfBuffers = 4
            };


            _waveIn.DataAvailable += async (s, a) =>
            {
                if (ct.IsCancellationRequested) return;


                // 들어온 바이트 누적
                _buffer!.Write(a.Buffer, 0, a.BytesRecorded);


                int bytesPerSample = BitsPerSample / 8;
                int chunkBytes = ChunkSamples * bytesPerSample * Channels;


                // 4096 샘플 단위로 잘라서 전송
                while (_buffer.Length >= chunkBytes)
                {
                    _buffer.Position = 0;
                    byte[] chunk = new byte[chunkBytes];
                    _buffer.Read(chunk, 0, chunkBytes);


                    // 남은 바이트를 앞으로 당김
                    var restLen = (int)(_buffer.Length - chunkBytes);
                    if (restLen > 0)
                    {
                        byte[] rest = new byte[restLen];
                        _buffer.Read(rest, 0, restLen);
                        _buffer.SetLength(0);
                        _buffer.Write(rest, 0, restLen);
                    }
                    else
                    {
                        _buffer.SetLength(0);
                    }

                    await onChunk(chunk);
                }
            };


            _waveIn.StartRecording();
            await Task.CompletedTask;
        }


        public void Stop()
        {
            try { _waveIn?.StopRecording(); } catch { }
        }


        public void Dispose()
        {
            _waveIn?.Dispose();
            _buffer?.Dispose();
        }
    }
}
