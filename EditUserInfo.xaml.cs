using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserAccountManager.Handlers;
using UserAccountManager.InterfacesWrapper;
using Utility;

namespace AICA_Client
{
    /// <summary>
    /// EditUserInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditUserInfo : Page
    {
        private MainWindow mainWnd;
        public EditUserInfo(MainWindow mainWnd)
        {
            InitializeComponent();
            this.mainWnd = mainWnd;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string alias = AliasInput.Text;
            string email = EmailInput.Text;
            string newPwd = NewPasswordInput.Password;
            string reNewPwd = ReNewPasswordInput.Password;

            (bool suc,string msg)=await UserInfoHandler.HandleSaveEditAsync(alias,email,newPwd,reNewPwd);
            MessageBox.Show(msg,"알림",MessageBoxButton.OK);
            if (suc) {
                this.mainWnd.OpenMyPageFrame();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainWnd.OpenMyPageFrame();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.mainWnd.CloseFrameContainer();
        }

        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string pwd = PasswordInput.Password;
            (bool suc,string msg)=await UserInfoHandler.HandlePasswordCheckAsync(pwd);
            if (suc)
            {
                CheckPasswordGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show(msg, "알림", MessageBoxButton.OK);
            }
        }

        private async void SendVerifyCodeButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailInput.Text;
            string result = await RegistrationInterface.SendEmailCode(email);
            //StartTimer();
            MessageBox.Show(result, "알림", MessageBoxButton.OK);
        }

        private async void CodeVerifyButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailInput.Text;
            string verifyCode = VerifyCodeInput.Text;
            (bool success, string result) = await RegistrationInterface.VerifyEmailCode(email, verifyCode);

            MessageBox.Show(result, "알림", MessageBoxButton.OK);
        }
    }
}
