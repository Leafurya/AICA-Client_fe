using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAccountManager.Helpers;
using UserAccountManager.Interfaces;
using UserAccountManager.Services;

namespace UserAccountManager.Handlers
{
    public static class LogoutHandler
    {
        public static async Task<(bool Success, string Message)> HandleLogoutAsync()
        {
            var (success, message) = await LogoutService.LogoutAsync();

            if (success)
            {
                TokenManager.SetTokens(null, null);
            }

            return (success, message);
        }
    }

}
