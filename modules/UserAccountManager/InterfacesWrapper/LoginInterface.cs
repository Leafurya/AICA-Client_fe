using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAccountManager.Handlers;
using UserAccountManager.Interfaces;

namespace UserAccountManager.InterfacesWrapper
{
    public static class LoginInterface
    {
        public static Task<(bool Success, string Message)> LoginAsync(ILoginView view)
        {
            return LoginHandler.HandleLoginAsync(view.UserId, view.Password);
        }
    }
}
