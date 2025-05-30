using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAccountManager.Services;

namespace UserAccountManager.Handlers
{
    public static class LoginHandler
    {
        public static async Task<(bool Success, string Message)> HandleLoginAsync(string userId, string password)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return (false, "아이디를 입력해주세요.");

            if (string.IsNullOrWhiteSpace(password))
                return (false, "비밀번호를 입력해주세요.");

            var (success, message) = await LoginService.LoginAsync(userId, password);
            return (success, message);
        }
    }

}
