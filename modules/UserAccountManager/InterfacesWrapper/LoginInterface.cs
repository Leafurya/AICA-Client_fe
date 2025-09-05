using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UserAccountManager.Handlers;
using UserAccountManager.Interfaces;

namespace UserAccountManager.InterfacesWrapper
{
    [Obsolete]
    public static class LoginInterface
    {
        public static async void LoginAsync(string userId,string pwd)
        {
            //(bool Success, string Message)= await LoginHandler.HandleLoginAsync(userId, pwd);
            
            //if (Success)
            //{

            //}
        }
    }
}
