using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Utility
{
    namespace RequestConst
    {
        public class RequestConst
        {
            static public HttpClient client=new HttpClient();
            static public string host="http://127.0.0.1:8080";
        }
    }
}
