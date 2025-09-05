using CustomControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Page1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Login : Page
    {
        private MainWindow mainWnd;
        public Login(MainWindow mainWnd)
        {
            InitializeComponent();
            this.mainWnd = mainWnd;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.mainWnd.CloseFrameContainer();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string id=textBox_id.Text;
            string pwd = textBox_pwd.Password;
            LoginAsync(id, pwd);
        }
        private async void LoginAsync(string userId, string pwd)
        {
            bool rememberMe = (bool)RememberMeToggleButton.IsChecked;
            (bool Success, string Message) = await LoginHandler.HandleLoginAsync(userId, pwd, rememberMe);
            Debug.WriteLine(Message + Success);
            if (!Success)
            {
                if (this.DataContext is SharedViewModel vm)
                {
                    vm.IsLogin = false;
                }
                MessageBox.Show(Message,"알림", MessageBoxButton.OK);
            }
            else
            {
                Debug.WriteLine("this.DataContext is SharedViewModel vm " + (this.DataContext is SharedViewModel ));
                if(this.DataContext is SharedViewModel vm)
                {
                    vm.IsLogin = true;
                }
                this.mainWnd.CloseFrameContainer();
            }
        }


        private void RegistButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainWnd.OpenRegistFrame();
        }

        private void RememberMeToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Utility.UserSetting.Interface.UpdateRememberMe(true);
        }

        private void RememberMeToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Utility.UserSetting.Interface.UpdateRememberMe(false);
        }
    }
}
