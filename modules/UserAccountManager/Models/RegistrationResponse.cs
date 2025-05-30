using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAccountManager.Models
{
    public class RegistrationResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

}
