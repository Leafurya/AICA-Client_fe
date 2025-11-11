using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Utility
{
    namespace RequestConst
    {
        public class AppConfig
        {
            public string host { get; set; } = "http://127.0.0.1:8080";
            public string wsEntryPoint { get; set; } = "ws://127.0.0.1/ws/pronunciation";
        }
        public class RequestConst
        {
            static public HttpClient client=new HttpClient();
            static public string host;
            static public string wsEntryPoint ;
            static public void LoadConst()
            {
                try
                {
                    string json = File.ReadAllText("config.json");
                    AppConfig config = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
                    host = config.host;
                    wsEntryPoint = config.wsEntryPoint;
                }catch(Exception ex)
                {
                    host = "http://127.0.0.1:8080";
                    wsEntryPoint = "ws://127.0.0.1/ws/pronunciation";
                }
            }
        }
    }
}
