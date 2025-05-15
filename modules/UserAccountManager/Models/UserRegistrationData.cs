using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAccountManager.Models
{
    public class UserRegistrationData
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public bool AgreeMarketing { get; set; }
    }
}
