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

namespace AICA_Client
{
    /// <summary>
    /// PwdConfirmPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PwdConfirmPage : Page
    {
        private MainWindow mainWnd;
        public PwdConfirmPage(MainWindow mainWnd)
        {
            InitializeComponent();
            this.mainWnd = mainWnd;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.mainWnd.CloseFrameContainer();
        }

        private void ViewPwdBtn_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox pwd = PasswordTextBox; //this.FindName("RegPwdBox") as PasswordBox;
            TextBox txt = PasswordTextBoxVisible;

            if (pwd.Visibility == Visibility.Visible)
            {
                txt.Text = pwd.Password;
                pwd.Visibility = Visibility.Collapsed;
                txt.Visibility = Visibility.Visible;
            }
            else
            {
                pwd.Password = txt.Text;
                txt.Visibility = Visibility.Collapsed;
                pwd.Visibility = Visibility.Visible;
            }
        }
        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string pwd = PasswordTextBox.Password;
            (bool suc, string msg) = await UserInfoHandler.HandlePasswordCheckAsync(pwd);
            if (suc)
            {
                this.mainWnd.OpenEditInfoFrame();
            }
            else
            {
                MessageBox.Show(msg, "알림", MessageBoxButton.OK);
            }
        }
    }
}
