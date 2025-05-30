using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UserAccountManager.Interfaces
{
    public interface IUserRegistrationView
    {
        string UserId { get; }
        string Password { get; }
        string ConfirmPassword { get; }
        string Nickname { get; }
        string Email { get; }
        string AuthCode { get; }

        bool IsTermsAgreed1 { get; }
        bool IsTermsAgreed2 { get; }
        bool IsTermsAgreed3 { get; }
    }

    public interface ILoginView
    {
        string UserId { get; }
        string Password { get; }
    }

    public interface IUserInfoEditView
    {
        string PasswordForCheck { get; }
        string Nickname { get; set; }
        string Email { get; set; }
        string NewPassword { get; }
        string ConfirmPassword { get; }

        void SetEditMode(bool enabled); // true면 편집 가능, false면 보기모드
        void ShowPasswordCheckPanel(bool visible); // 비밀번호 입력창 표시/숨김
        void ShowMessage(string message);
    }


    public interface ILogoutView
    {
        System.Windows.Controls.RichTextBox UserIdBox { get; }
        System.Windows.Controls.PasswordBox PasswordBox { get; }

        void NavigateToLogin();
    }
    public interface IUserInfoView
    {
        void SetUserId(string userId);
        void SetNickname(string nickname);
        void SetEmail(string email);
        void ShowMessage(string message);
    }


}
