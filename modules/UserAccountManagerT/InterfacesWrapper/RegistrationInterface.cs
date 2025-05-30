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
    public class RegistrationInterface
    {
        public static async Task HandleIdCheck(IUserRegistrationView view)
        {
            await UserRegistrationHandler.HandleIdCheckAsync(view.UserId, view.ShowMessage);
        }

        public static async Task SendEmailCode(IUserRegistrationView view)
        {
            await EmailVerificationHandler.HandleSendCodeAsync(
                view.Email,
                view.ShowMessage,
                view.SetTimerText,
                view.SetSendButtonText
            );
        }

        public static async Task VerifyEmailCode(IUserRegistrationView view)
        {
            await EmailVerificationHandler.HandleVerifyCodeAsync(
                view.Email,
                view.AuthCode,
                view.ShowMessage
            );
        }

        public static async Task RegisterUser(IUserRegistrationView view)
        {
            await UserRegistrationHandler.HandleRegisterAsync(
                view.UserId,
                view.Password,
                view.ConfirmPassword,
                view.Nickname,
                view.Email,
                view.AuthCode,
                view.IsTermsAgreed1,
                view.IsTermsAgreed2,
                view.IsTermsAgreed3,
                view.ShowMessage
            );
        }

    }
}
