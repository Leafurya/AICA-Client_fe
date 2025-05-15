using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAccountManager.Models
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string CreatedAt { get; set; } // yyyy-MM-dd 포맷 추천
    }

}
