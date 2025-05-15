using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAccountManager.Handlers;
using UserAccountManager.Interfaces;

namespace UserAccountManager.InterfacesWrapper
{
    public static class UserInfoInterface
    {
        public static async Task LoadUserInfoAsync(IUserInfoView view)
        {
            await UserInfoHandler.HandleLoadUserInfoAsync(view);
        }
    }
}
