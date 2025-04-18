using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;


namespace Translate
{
    public class Translator
    {
        public static async Task<string> TranslateAsync(string text)
        {
            string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "translate.exe");


            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = $"\"{text}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            string result = "";
            using (Process process = Process.Start(psi))
            {
                result = await process.StandardOutput.ReadToEndAsync();
                process.WaitForExit();  
            }


            return result.Trim();
        }
    }
}
