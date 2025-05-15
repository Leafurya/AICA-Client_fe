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
        public static async Task HandleLoginAsync(string userId, string password, Action<string> notify)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                notify("아이디를 입력해주세요.");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                notify("비밀번호를 입력해주세요.");
                return;
            }
            
            var (success, message) = await LoginService.LoginAsync(userId, password);
            notify(message);
        }
    }

}
