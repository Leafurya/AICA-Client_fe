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
    public class UserRegistrationHandler
    {
        public static async Task HandleIdCheckAsync(string userId, Action<string> notify)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                notify("아이디를 입력해주세요.");
                return;
            }

            bool isDuplicate = await UserRegistrationService.IsIdDuplicateAsync(userId);

            if (isDuplicate)
                notify("이미 사용 중인 아이디입니다.");
            else
                notify("사용 가능한 아이디입니다.");
        }

        public static async Task HandleRegisterAsync(
           string userId,
           string password,
           string confirmPassword,
           string nickname,
           string email,
           string authCode,
           bool isTermsChecked1,
           bool isTermsChecked2,
            bool isTermsChecked3,
           Action<string> notify)
        {
            if (string.IsNullOrWhiteSpace(userId) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword) ||
                string.IsNullOrWhiteSpace(nickname) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(authCode))
            {
                notify("모든 필드를 입력해주세요.");
                return;
            }

            if (!isTermsChecked1 || !isTermsChecked2)
            {
                notify("필수 약관에 동의해주세요.");
                return;
            }

            var pwResult = UserRegistrationValidator.ValidatePasswords(password, confirmPassword);
            if (!pwResult.IsValid)
            {
                notify(pwResult.ErrorMessage);
                return;
            }

            var emailResult = UserRegistrationValidator.ValidateEmail(email);
            if (!emailResult.IsValid)
            {
                notify(emailResult.ErrorMessage);
                return;
            }

            // 이메일 인증 여부만 확인
            if (!EmailVerificationHandler.IsVerified)
            {
                notify("이메일 인증이 완료되지 않았습니다.");
                return;
            }

            var registrationData = new UserRegistrationData
            {
                UserId = userId,
                Password = password,
                Nickname = nickname,
                Email = email,
                AgreeMarketing = isTermsChecked3
            };

            var (success, message) = await UserRegistrationService.RegisterUserAsync(registrationData);

            if (success)
            {
                notify("회원가입 완료!");
                EmailVerificationHandler.IsVerified = false; //  인증 상태 초기화
            }
            else
            {
                notify($"회원가입 실패: {message}");
            }
        }
    }
}
