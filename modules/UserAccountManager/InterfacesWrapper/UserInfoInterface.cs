using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAccountManager.Handlers;
using UserAccountManager.Interfaces;
using UserAccountManager.Models;

namespace UserAccountManager.InterfacesWrapper
{
    public static class UserInfoInterface
    {
        public static Task LoadUserInfoAsync(IUserInfoView view)
        {
            return UserInfoHandler.HandleLoadUserInfoAsync(view);
        }
    }
}
