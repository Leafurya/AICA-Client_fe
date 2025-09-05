using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using UserAccountManager.Helpers;

namespace UserAccountManager.Services
{
    public static class EmailVerificationHandler
    {
        //public static int SentCode { get; private set; }
        public static bool IsVerified { get; set; } = false;

        public static async Task<(bool Success, string Message)> HandleSendCodeAsync(string email)
        {
            var result = UserRegistrationValidator.ValidateEmail(email);
            if (!result.IsValid)
                return (false, result.ErrorMessage);

            var (success, message) = await EmailService.RequestAuthCodeFromServerAsync(email);
            if (!success)
                return (false, message);

            //SentCode = code;
            IsVerified = false;
            return (true, message);
        }

        public static async Task<(bool Success, string Message)> HandleVerifyCodeAsync(string email, string userInputCode)
        {
            var (success, message) = await EmailService.VerifyCodeWithServerAsync(email, userInputCode);

            if (success)
                Debug.WriteLine("이메일 인증 성공 "+success);
                IsVerified = true;

            return (success, message);
        }
    }
}
