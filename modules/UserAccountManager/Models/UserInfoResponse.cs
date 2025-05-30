using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAccountManager.Models
{
    public class UserInfoData
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserNickname { get; set; }
    }

    public class UserInfoResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public UserInfoData Data { get; set; }
    }


}
