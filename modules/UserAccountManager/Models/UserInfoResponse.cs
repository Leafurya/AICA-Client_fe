using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAccountManager.Models
{
    public class UserInfoData
    {
        public int id {  get; set; }
        public string userId { get; set; }
        public string email { get; set; }
        public string nickname { get; set; }
    }

    public class UserInfoResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public UserInfoData Data { get; set; }
    }


}
