using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAccountManager.Models
{
    public class LogoutResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public LogoutData? Data { get; set; }
    }
    public class LogoutData
    {
        public int UserId { get; set; }
    }


}
