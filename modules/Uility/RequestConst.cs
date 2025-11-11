using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Security.Policy;

namespace Utility
{
    namespace RequestConst
    {
        public class RequestConst
        {
            static public HttpClient client=new HttpClient();
            //static public string host = "http://127.0.0.1";
            //static public string wsEntryPoint = "ws://127.0.0.1/ws/pronunciation";
            static public string host = "http://52.79.93.151";
            static public string wsEntryPoint = "ws://52.79.93.151/ws/pronunciation";
            //static public string host = "http://127.0.0.1:8080";
            //static public string wsEntryPoint = "ws://127.0.0.1/ws/pronunciation";
        }
    }
}
