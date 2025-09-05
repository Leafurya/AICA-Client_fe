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
            static public string host="http://3.38.30.238";
            //static public string host = "http://127.0.0.1:8080";
        }
    }
}
