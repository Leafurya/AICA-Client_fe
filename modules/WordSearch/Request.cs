using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WordSearch
{
    /// <summary>
    /// RestAPI 요청을 위한 클래스
    /// </summary>
    class HttpRequest
    {
        private HttpClient client = new HttpClient();
        private string host;

        /// <summary>
        /// RestAPI 대상 host 지정
        /// </summary>
        /// <param name="host">
        /// RestAPI를 보낼 서버의 주소<br/>
        /// ex) https://127.0.0.1:8080
        /// </param>
        public HttpRequest(string host)
        {
            this.host = host;
        }
        public async Task RequestDictionary(string word)
        {
            //url 예시: host/dictionary?word=hello
            HttpResponseMessage res = await client.GetAsync(this.host + "/" + word);
            if (res.IsSuccessStatusCode)
            {
                string result = await res.Content.ReadAsStringAsync();
                Debug.WriteLine(result);
            }
        }
    }
}
