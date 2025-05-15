using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAccountManager.Interfaces;
using UserAccountManager.Services;

namespace UserAccountManager.Handlers
{
    public static class UserInfoHandler
    {
        public static async Task HandleLoadUserInfoAsync(IUserInfoView view)
        {
            // ✅ 토큰 자동 검증 및 리프레시 처리
            if (!await TokenManager.EnsureValidTokenAsync())
            {
                view.ShowError("세션이 만료되었습니다. 다시 로그인해주세요.");
                return;
            }

            var (success, data, message) = await UserInfoService.GetUserInfoAsync();

            if (success)
                view.ShowUserInfo(data.Nickname, data.Email, data.CreatedAt);
            else
                view.ShowError(message);
        }
    }
}
