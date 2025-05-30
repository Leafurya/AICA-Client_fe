using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using System.Text.RegularExpressions;

namespace UserAccountManager.Helpers
{   
        public static class UserRegistrationValidator
        {
            public static (bool IsValid, string ErrorMessage) ValidatePasswords(string password, string confirmPassword)
            {
                if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
                    return (false, "비밀번호를 모두 입력해주세요.");

                if (password.Length < 5 || password.Length >= 15)
                    return (false, "비밀번호는 5자 이상 15자 미만이어야 합니다.");

                if (password != confirmPassword)
                    return (false, "비밀번호가 일치하지 않습니다.");

                return (true, "");
            }

            public static (bool IsValid, string ErrorMessage) ValidateEmail(string email)
            {
                if (string.IsNullOrWhiteSpace(email))
                    return (false, "이메일을 입력해주세요.");

                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(email, pattern))
                    return (false, "유효하지 않은 이메일 형식입니다.");

                return (true, "");
            }
            public static (bool IsValid, string ErrorMessage) ValidateTerms(bool agree1, bool agree2)
            {
                if (!agree1 || !agree2)
                    return (false, "필수 약관에 모두 동의해야 회원가입이 가능합니다.");

                    return (true, "");
            }

    }
    

}
