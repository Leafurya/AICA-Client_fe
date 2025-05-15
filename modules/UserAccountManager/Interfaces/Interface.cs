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

        void ShowMessage(string message);
        void SetTimerText(string text);
        void SetSendButtonText(string text);
    }

    public interface ILoginView
    {
        string UserId { get; }
        string Password { get; }
        void ShowMessage(string message);
    }
    public interface IUserInfoView
    {
        void ShowUserInfo(string nickname, string email, string createdAt);
        void ShowError(string message);
    }


    public interface ILogoutView
    {
        RichTextBox UserIdBox { get; }
        RichTextBox PasswordBox { get; }
        void ShowMessage(string message);
        void NavigateToLogin();
    }

    public interface IUserInfoEditView
    {
        string PasswordForCheck { get; }
        string Nickname { get; set; }

        void SetEditMode(bool enabled); // true면 편집 가능, false면 보기모드
        void ShowPasswordCheckPanel(bool visible);
        void ShowMessage(string msg);
    }

}
