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
            app = new ChildPipeClient(); // 자식 프로세스 실행
            return Task.CompletedTask;
        }
        // 자식 프로세스 종료
        static public void Close() 
        {
            while (app == null);
            app.Dispose();
        }
        // 실행 확인용 에코
        static public void Echo()
        {
            object json = new
            {
                msg = "hello third party"
            };
            using (var resp = app.Call("echo", json))
                Debug.WriteLine(resp.RootElement.ToString());
        }
        // 문장 분석 요청
        static public int AnalyzeSentence(string sentence)
        {
            int result = -1;
            object json = new
            {
                text = sentence
            };
            using (JsonDocument resp = app.Call("analyze", json)) // 자식 프로세스로 문장 분석 요청 메시지 전송
            {
                try
                {
                    result = resp.RootElement.GetProperty("textId").GetInt32();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);

                }
            }
            return result;
        }
        // 텍스트 추출 요청
        static public string ExtractText(string imgPath)
        {
            string result="";

            object json = new
            {
                imgPath = imgPath
            };
            using (JsonDocument resp = app.Call("extract", json)) // 자식 프로세스로 텍스트 추출 요청 메시지 전송
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
        // 번역 요청
        static public string TranslateText(string text)
        {
            string result = "";
            object json = new
            {
                text = text
            };
            using (JsonDocument resp = app.Call("translate", json)) // 자식 프로세스로 번역 요청 메시지 전송
            {
                try {
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
