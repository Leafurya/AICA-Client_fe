using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAccountManager.Interfaces;
using UserAccountManager.Services;

namespace UserAccountManager.Handlers
{
    public static class UserInfoUpdateHandler
    {
       public static async Task<(bool Success, string Message)> TryEnterEditModeAsync(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return (false, "비밀번호를 입력해주세요.");
            }

            var (success, message) = await UserInfoService.VerifyPasswordAsync(password);

            return success
                ? (true, "비밀번호 확인 성공")
                : (false, "비밀번호가 일치하지 않습니다.");
        }
    }
}
