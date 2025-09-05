using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UserAccountManager.Handlers;
using UserAccountManager.Interfaces;
using UserAccountManager.Services;

namespace UserAccountManager.InterfacesWrapper
{
    public static class RegistrationInterface
    {
        public static async Task<string> HandleIdCheck(string id)
        {
            //return UserRegistrationHandler.HandleIdCheckAsync(view.UserId);
            (bool Success, string Message) = await UserRegistrationHandler.HandleIdCheckAsync(id);
            return Message;
        }

        public static async Task<string> SendEmailCode(string email)
        {
            (bool Success, string Message) = await EmailVerificationHandler.HandleSendCodeAsync(email);
            return Message;
        }

        public static async Task<(bool,string)> VerifyEmailCode(string email, string verifyCode)
        {
            (bool Success, string Message) = await EmailVerificationHandler.HandleVerifyCodeAsync(email, verifyCode);
            return (Success,Message);
        }

        //public static Task<(bool Success, string Message)> RegisterUser(IUserRegistrationView view)
        //{
        //    return UserRegistrationHandler.HandleRegisterAsync(
        //        view.UserId,
        //        view.Password,
        //        view.ConfirmPassword,
        //        view.Nickname,
        //        view.Email,
        //        view.AuthCode,
        //        view.IsTermsAgreed1,
        //        view.IsTermsAgreed2,
        //        view.IsTermsAgreed3);
        //}
        public static async Task<(bool Success, string Message)> RegistUser(string userId, string pwd,string rePwd, string email, string alias, string verifyCode,bool agree1,bool agree2,bool agree3)
        {
            //(bool Success, string Message) = await UserRegistrationHandler.HandleIdCheckAsync(userId);
            //if (!Success)
            //{
            //    return (Success, Message);
            //}
            (bool Success, string Message) = await UserRegistrationHandler.HandleRegisterAsync(userId, pwd, rePwd, alias, email, verifyCode, agree1, agree2, agree3);
            //if (!Success)
            //{
            //    return (Success, Message);
            //}
            return (Success, Message);
        }
    }
}
