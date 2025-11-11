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
        public static async Task<(bool Success, string Message)> RegistUser(string userId, string pwd,string rePwd, string email, string alias, string verifyCode,bool agree1,bool agree2,bool agree3)
        {
            (bool Success, string Message) = await UserRegistrationHandler.HandleRegisterAsync(userId, pwd, rePwd, alias, email, verifyCode, agree1, agree2, agree3);
            return (Success, Message);
        }
    }
}
