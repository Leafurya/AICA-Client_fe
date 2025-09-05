using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UserAccountManager.Interfaces;
using UserAccountManager.Services;
using Utility.TokenManager;

namespace UserAccountManager.Handlers
{
    public static class UserDeleteHandler
    {
         public static async Task<(bool,string)> HandleDeleteAsync()
         {
            (bool suc, string msg)= await UserDeleteService.DeleteUserAsync();
            if (suc)
            {
                TokenManager.SetTokens(null, null);
            }
            return (suc, msg);
             //view.ShowMessage(message);
             //return success;
         }
        //public static async Task<bool> HandleDeleteAsync()
        //{
        //    await Task.Delay(100); // 테스트용 딜레이
        //    MessageBox.Show("회원 탈퇴가 완료되었습니다.");
        //    return true;
        //}
    }

}
