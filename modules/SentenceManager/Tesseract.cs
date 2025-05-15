using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SentenceManager
{
    class Tesseract
    {
        public string GetString(string imgPath)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "Tesseract.exe",
                Arguments = "\"eng\" \"" + imgPath + "\"",
                RedirectStandardOutput = true,  // 표준 출력 리디렉션
                RedirectStandardError = true,   // 표준 에러 리디렉션 (선택)
                UseShellExecute = false,        // 반드시 false여야 리디렉션 가능
                CreateNoWindow = true,      // 창을 띄우지 않음
                StandardOutputEncoding = Encoding.UTF8, // 추가
                StandardErrorEncoding = Encoding.UTF8   // 추가
            };

            using (var process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();  // 표준 출력 읽기
                string error = process.StandardError.ReadToEnd();    // 표준 에러 읽기 (옵션)
                process.WaitForExit();  // 프로세스가 끝날 때까지 대기
                string resultFileName = "ocroutput.txt";
                while (!File.Exists(resultFileName));
                if (File.Exists(resultFileName))
                {
                    string result = File.ReadAllText(resultFileName, System.Text.Encoding.UTF8);
                    File.Delete(resultFileName);
                    return result;
                }
                else
                {
                    Debug.WriteLine("404 file not found");
                    return "";
                }
            }
        }
    }
}
