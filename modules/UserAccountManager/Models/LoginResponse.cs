using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAccountManager.Models
{
    public class LoginResponse
    {
        public bool Success { get; set; }            // ✅ 서버에서 success 플래그 반환
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
