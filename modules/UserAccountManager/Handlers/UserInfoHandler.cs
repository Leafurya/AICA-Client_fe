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
                //view.ShowMessage(tokenMessage);
                return tokenMessage;
            }

            var (success, message, data) = await UserInfoService.GetUserInfoAsync();
            if (!success || data == null)
            {
                //view.ShowMessage(message);
                return tokenMessage;
            }
            result.nickname = data.nickname;
            result.userId = data.userId;
            result.email = data.email;
            result.id = data.id;
            //id = data.userId;
            //email = data.email;
            //alias = data.nickname;
            return null;
            //view.SetUserId(data.UserId);
            //view.SetEmail(data.UserEmail);
            //view.SetNickname(data.UserNickname);
        }

        public static async Task<(bool,string)> HandlePasswordCheckAsync(string password)
        {
            return await UserInfoUpdateHandler.TryEnterEditModeAsync(password);

            //if (!Success)
            //{
            //    view.ShowMessage(Message);
            //    return false;
            //}

            //return true;
        }

        public static async Task<(bool,string)> HandleSaveEditAsync(
    string nickname,
    string email,
    string newPassword,
    string confirmPassword)
        {
            //if (string.IsNullOrWhiteSpace(nickname) || string.IsNullOrWhiteSpace(email))
            //{
            //    //view.ShowMessage("닉네임과 이메일을 입력해주세요.");
            //    return (false, "닉네임과 이메일을 입력해주세요.");
            //}

            // 새 비밀번호가 있을 때만 비밀번호 변경 시도
            //if (!string.IsNullOrEmpty(newPassword) || !string.IsNullOrEmpty(confirmPassword))
            //{
            //    var pwResult = await UserInfoUpdateService.UpdatePasswordAsync(
            //        currentPassword: null,
            //        newPassword,
            //        confirmPassword
            //    );

            //    if (!pwResult.Success)
            //    {
            //        //view.ShowMessage(pwResult.Message);
            //        return (false, pwResult.Message);
            //    }
            //}
            

            var updateResult = await UserInfoUpdateService.UpdateUserInfoAsync(nickname, email, newPassword, confirmPassword);

            if (!updateResult.Success)
            {
                //view.ShowMessage(updateResult.Message);
                return (false, updateResult.Message);
            }

            return (true, updateResult.Message);
        }


    }
}
