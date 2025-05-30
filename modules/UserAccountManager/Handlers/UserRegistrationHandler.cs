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

            var data = new UserRegistrationData
            {
                UserId = userId,
                Password = password,
                Nickname = nickname,
                Email = email,
                AgreeMarketing = isTermsChecked3
            };

            return await UserRegistrationService.RegisterUserAsync(data);
        }

        public static async Task<(bool Success, string Message)> HandleIdCheckAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return (false, "아이디를 입력해주세요.");

            bool isDuplicate = await UserRegistrationService.IsIdDuplicateAsync(userId);

            return isDuplicate
                ? (false, "이미 사용 중인 아이디입니다.")
                : (true, "사용 가능한 아이디입니다.");
        }
    }
}
