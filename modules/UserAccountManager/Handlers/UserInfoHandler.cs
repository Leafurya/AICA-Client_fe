using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAccountManager.Interfaces;
using UserAccountManager.Services;
using UserAccountManager.Models;

namespace UserAccountManager.Handlers
{
    public static class UserInfoHandler
    {
        public static async Task HandleLoadUserInfoAsync(IUserInfoView view)
         {
             var (valid, tokenMessage) = await TokenManager.EnsureValidTokenAsync();
             if (!valid)
             {
                 view.ShowMessage(tokenMessage);
                 return;
             }

             var (success, message, data) = await UserInfoService.GetUserInfoAsync();
             if (!success || data == null)
             {
                 view.ShowMessage(message);
                 return;
             }

             view.SetUserId(data.UserId);
             view.SetEmail(data.UserEmail);
             view.SetNickname(data.UserNickname);
         }

        public static async Task<bool> HandlePasswordCheckAsync(string password, IUserInfoEditView view)
        {
            var (Success, Message) = await UserInfoUpdateHandler.TryEnterEditModeAsync(password);

            if (!Success)
            {
                view.ShowMessage(Message);
                return false;
            }

            return true;
        }

        public static async Task<bool> HandleSaveEditAsync(
    string nickname,
    string email,
    string newPassword,
    string confirmPassword,
    IUserInfoEditView view)
        {
            if (string.IsNullOrWhiteSpace(nickname) || string.IsNullOrWhiteSpace(email))
            {
                view.ShowMessage("닉네임과 이메일을 입력해주세요.");
                return false;
            }

            // 새 비밀번호가 있을 때만 비밀번호 변경 시도
            if (!string.IsNullOrEmpty(newPassword) || !string.IsNullOrEmpty(confirmPassword))
            {
                var pwResult = await UserInfoUpdateService.UpdatePasswordAsync(
                    currentPassword: null,
                    newPassword,
                    confirmPassword
                );

                if (!pwResult.Success)
                {
                    view.ShowMessage(pwResult.Message);
                    return false;
                }
            }

            var updateResult = await UserInfoUpdateService.UpdateUserInfoAsync(nickname, email);

            if (!updateResult.Success)
            {
                view.ShowMessage(updateResult.Message);
                return false;
            }

            return true;
        }


    }
}
