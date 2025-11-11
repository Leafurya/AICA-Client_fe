
using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace ThirdParty
{

    public class Interface
    {
        static private ChildPipeClient app;
        static public Task Init()
        {
            try
            {
                app = new ChildPipeClient("E:\\DevTools\\Anaconda\\envs\\capstone-thirdparty\\python.exe", "E:\\GitHub\\capstone\\thirdparty\\main.py");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("자식프로세스를 찾지 못했습니다.\n./child/child.exe", "에러");
                throw ex;
            }
            return Task.CompletedTask;
        }
        static public void Echo()
        {
            //Debug.WriteLine("start echo");
            object json = new
            {
                msg = "hello third party"
            };
            //byte[] response=app.Send(json);
            using (var resp = app.Call("echo", json))
                Debug.WriteLine(resp.RootElement.ToString());
        }
        static public int AnalyzeSentence(string sentence)
        {
            int result = -1;
            object json = new
            {
                text = sentence
            };
            using (JsonDocument resp = app.Call("analyze", json))
            {
                try
                {
                    string pretty = JsonSerializer.Serialize(
                                    resp.RootElement,
                                    new JsonSerializerOptions { WriteIndented = true }
);

                    Debug.WriteLine(pretty);
                    result = resp.RootElement.GetProperty("textId").GetInt32();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);

                }
            }
            return result;
        }
        static public string ExtractText(string imgPath)
        {
            string result="";

            object json = new
            {
                imgPath = imgPath
            };
            using (JsonDocument resp = app.Call("extract", json))
            {
                try
                {
                    result = resp.RootElement.GetProperty("text").GetString();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(resp.RootElement.ToString());
                }
            }

            return result;
        }
        static public string TranslateText(string text)
        {
            string result = "";
            object json = new
            {
                text = text
            };
            using (JsonDocument resp = app.Call("translate", json))
            {
                try {

                    string pretty = JsonSerializer.Serialize(
                                    resp.RootElement,
                                    new JsonSerializerOptions { WriteIndented = true }
);

                    Debug.WriteLine(pretty);
                    result = resp.RootElement.GetProperty("text").GetString();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    
                }
            }
            return result;
        }
    }
}
