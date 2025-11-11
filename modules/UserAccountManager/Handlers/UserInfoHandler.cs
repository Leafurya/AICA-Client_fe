using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAccountManager.Interfaces;
using UserAccountManager.Services;
using UserAccountManager.Models;
using Utility.TokenManager;
using System.Diagnostics;

namespace UserAccountManager.Handlers
{
    public static class UserInfoHandler
    {
        public static async Task<string?> HandleLoadUserInfoAsync(UserInfoData result)
        {
            var (valid, tokenMessage) = await TokenManager.EnsureValidTokenAsync();
            if (!valid)
            {
                Debug.WriteLine("valid " + valid);
                return tokenMessage;
            }

            var (success, message, data) = await UserInfoService.GetUserInfoAsync();
            if (!success || data == null)
            {
                return tokenMessage;
            }
            result.nickname = data.nickname;
            result.userId = data.userId;
            result.email = data.email;
            result.id = data.id;
            return null;
        }

        public static async Task<(bool,string)> HandlePasswordCheckAsync(string password)
        {
            return await UserInfoUpdateHandler.TryEnterEditModeAsync(password);
        }

        public static async Task<(bool,string)> HandleSaveEditAsync(
    string nickname,
    string email,
    string newPassword,
    string confirmPassword)
        {
            var updateResult = await UserInfoUpdateService.UpdateUserInfoAsync(nickname, email, newPassword, confirmPassword);

            if (!updateResult.Success)
            {
                return (false, updateResult.Message);
            }

            return (true, updateResult.Message);
        }


    }
}
