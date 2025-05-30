using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAccountManager.Models
{
    public class LoginData
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class LoginResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public LoginData Data { get; set; }
    }
}
