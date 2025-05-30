using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAccountManager.Helpers;
using UserAccountManager.Interfaces;
using UserAccountManager.Services;

namespace UserAccountManager.Handlers
{
    public static class LogoutHandler
    {
        public static void HandleLogout(ILogoutView view)
        {
            // 1. 토큰 초기화
            TokenManager.SetTokens(null, null);

            // 2. 입력창 초기화 (RichTextBox 기준)
            UIResetHelper.ResetLoginFields(view.UserIdBox, view.PasswordBox);

            // 3. 메시지 출력 및 화면 전환
            view.ShowMessage("로그아웃 되었습니다.");
            view.NavigateToLogin();
        }
    }
}
