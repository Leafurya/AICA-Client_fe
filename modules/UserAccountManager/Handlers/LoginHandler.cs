using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAccountManager.Services;
//using Utility.TokenManager;

namespace UserAccountManager.Handlers
{
    public static class LoginHandler
    {
        public static async Task<(bool Success, string Message)> HandleLoginAsync(string userId, string password,bool rememberMe)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return (false, "아이디를 입력해주세요.");

            if (string.IsNullOrWhiteSpace(password))
                return (false, "비밀번호를 입력해주세요.");

            var (success, message) = await LoginService.LoginAsync(userId, password, rememberMe);

            if (success&&rememberMe)
            {
                Utility.TokenManager.TokenManager.SaveTokensToDB();
            }

            return (success, message);
        }
        public static async Task<(bool Success, string Message)> AutoLogin()
        {
            if (!Utility.UserSetting.Interface.IsRememberMe())
            {
                return (false, "remember me is false");
            }
            Utility.TokenManager.TokenManager.LoadTokens();

            bool succ;
            string msg;
            (succ,msg)=await Utility.TokenManager.TokenManager.RefreshTokenAsync();

            if (succ)
            {
                Utility.TokenManager.TokenManager.SaveTokensToDB();
            }

            return (succ, msg);
        }
    }

}
