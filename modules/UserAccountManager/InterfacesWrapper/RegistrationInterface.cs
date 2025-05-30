using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAccountManager.Handlers;
using UserAccountManager.Interfaces;
using UserAccountManager.Services;

namespace UserAccountManager.InterfacesWrapper
{
    public static class RegistrationInterface
    {
        public static Task<(bool Success, string Message)> HandleIdCheck(IUserRegistrationView view)
        {
            return UserRegistrationHandler.HandleIdCheckAsync(view.UserId);
        }

        public static Task<(bool Success, string Message)> SendEmailCode(IUserRegistrationView view)
        {
            return EmailVerificationHandler.HandleSendCodeAsync(view.Email);
        }

        public static Task<(bool Success, string Message)> VerifyEmailCode(IUserRegistrationView view)
        {
            return EmailVerificationHandler.HandleVerifyCodeAsync(view.Email, view.AuthCode);
        }

        public static Task<(bool Success, string Message)> RegisterUser(IUserRegistrationView view)
        {
            return UserRegistrationHandler.HandleRegisterAsync(
                view.UserId,
                view.Password,
                view.ConfirmPassword,
                view.Nickname,
                view.Email,
                view.AuthCode,
                view.IsTermsAgreed1,
                view.IsTermsAgreed2,
                view.IsTermsAgreed3);
        }
    }
}
