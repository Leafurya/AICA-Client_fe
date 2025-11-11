using CustomControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
using UserAccountManager.Models;

namespace AICA_Client
{
    /// <summary>
    /// MyPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MyPage : Page
    {
        private MainWindow mainWnd;
        public MyPage(MainWindow mainWnd)
        {
            InitializeComponent();
            this.mainWnd = mainWnd;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.DataContext is SharedViewModel vm)
            {
                UserInfoData data=new UserInfoData();
                string? msg=await UserInfoHandler.HandleLoadUserInfoAsync(data);
                
                if (msg != null)
                {
                    MessageBox.Show(msg,"알림",MessageBoxButton.OK);
                }
                else
                {
                    vm.UserInfoID = data.userId;
                    vm.UserInfoEmail = data.email;
                    vm.UserInfoAlias = data.nickname;
                    Debug.WriteLine($"{vm.UserInfoID}, {vm.UserInfoEmail}, {vm.UserInfoAlias}");
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.mainWnd.CloseFrameContainer();
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            (bool suc,string msg)=await LogoutHandler.HandleLogoutAsync();
            //MessageBox.Show(msg, "알림", MessageBoxButton.OK);
            if (suc)
            {
                if (this.DataContext is SharedViewModel vm)
                {
                    vm.IsLogin = false;
                    vm.WordsList?.Clear();
                    vm.AicaList?.Clear();
                    VocabNote.Interface.Clear();
                    this.mainWnd.OpenLogoutSuccessPage();
                }
            }
        }

        private async void LeaveMemberButton_Click(object sender, RoutedEventArgs e)
        {
            (bool suc,string msg)=await UserDeleteHandler.HandleDeleteAsync();
            //MessageBox.Show(msg, "알림", MessageBoxButton.OK);
            if (suc)
            {
                if(this.DataContext is SharedViewModel vm)
                {
                    vm.IsLogin = false;
                    this.mainWnd.OpenQuitMemberSuccessPage();
                }
            }
        }

        private void EditInfoButton_Click(object sender, RoutedEventArgs e)
        {
            //this.mainWnd.OpenEditInfoFrame();
            this.mainWnd.OpenPwdConfirmPageFrame();
        }
    }
}
