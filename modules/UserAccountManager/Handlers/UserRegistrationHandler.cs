using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAccountManager.Helpers;
using UserAccountManager.Services;
using UserAccountManager.Models;

namespace UserAccountManager.Handlers
{
    public static class UserRegistrationHandler
    {
        private static bool doIdDuplicate=false;//test용으로 강제로 true로 지정함 
        public static async Task<(bool Success, string Message)> HandleRegisterAsync(
            string userId,
            string password,
            string confirmPassword,
            string nickname,
            string email,
            string authCode,
            bool isTermsChecked1,
            bool isTermsChecked2,
            bool isTermsChecked3)
        {
            if (string.IsNullOrWhiteSpace(userId) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword) ||
                string.IsNullOrWhiteSpace(nickname) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(authCode))
            {
                return (false, "모든 필드를 입력해주세요.");
            }

            var terms = UserRegistrationValidator.ValidateTerms(isTermsChecked1, isTermsChecked2);
            if (!terms.IsValid)
                return (false, terms.ErrorMessage);

            var pwResult = UserRegistrationValidator.ValidatePasswords(password, confirmPassword);
            if (!pwResult.IsValid)
                return (false, pwResult.ErrorMessage);

            var emailResult = UserRegistrationValidator.ValidateEmail(email);
            if (!emailResult.IsValid)
                return (false, emailResult.ErrorMessage);

            if (!EmailVerificationHandler.IsVerified)
                return (false, "이메일 인증이 완료되지 않았습니다.");

            if (!doIdDuplicate)
            {
                return (false, "아이디 중복 검사가 완료되지 않았습니다.");
            }

            var data = new UserRegistrationData
            {
                userId = userId,
                password = password,
                userNickname = nickname,
                email = email
            };

            EmailVerificationHandler.IsVerified = false;
            doIdDuplicate = false;
            return await UserRegistrationService.RegisterUserAsync(data);
        }

        public static async Task<(bool Success, string Message)> HandleIdCheckAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return (false, "아이디를 입력해주세요.");

            bool isDuplicate = await UserRegistrationService.IsIdDuplicateAsync(userId);

            doIdDuplicate = isDuplicate;
            return isDuplicate
                ? (true, "사용 가능한 아이디입니다.") :
                (false, "이미 사용 중인 아이디입니다.");
        }
    }
}
