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
using System.Windows.Threading;
using UserAccountManager.Handlers;
using UserAccountManager.InterfacesWrapper;

namespace AICA_Client
{
    /// <summary>
    /// Regist.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Regist : Page
    {
        private MainWindow mainWnd;
        public Regist(MainWindow mainWnd)
        {
            InitializeComponent();
            this.mainWnd = mainWnd;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.mainWnd.CloseFrameContainer();
        }

        private async void DoRegistButton_Click(object sender, RoutedEventArgs e)
        {
            //if (!emailCodeVerified)
            //{
            //    MessageBox.Show("인증번호를 확인해주세요.", "알림", MessageBoxButton.OK);
            //    return;
            //}
            string userId=UserIdTextBox.Text;
            string pwd = PasswordTextBox.Password;
            string rePwd =RePasswordTextBox.Password;
            string email=EmailTextBox.Text;
            string alias=AliasTextBox.Text;
            string verifyCode=VerifyCodeTextBox.Text;
            bool isTermsAgreed1 = (bool)AgreeCheckBox1.IsChecked;
            bool isTermsAgreed2 = (bool)AgreeCheckBox2.IsChecked;
            bool isTermsAgreed3 = (bool)AgreeCheckBox3.IsChecked;
            (bool success, string msg)=await RegistrationInterface.RegistUser(userId, pwd, rePwd, email, alias, verifyCode, isTermsAgreed1, isTermsAgreed2, isTermsAgreed3);
            MessageBox.Show(msg, "알림");
            Debug.WriteLine(success);
            if (success)
            {
                this.mainWnd.OpenLoginFrame();
            }
        }

        private async void VerifyIdButton_Click(object sender, RoutedEventArgs e)
        {
            string userId = UserIdTextBox.Text;
            string result=await RegistrationInterface.HandleIdCheck(userId);
            MessageBox.Show(result, "알림", MessageBoxButton.OK);
        }

        private async void SendVerifyCodeButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string result = await RegistrationInterface.SendEmailCode(email);
            StartTimer();
            MessageBox.Show(result, "알림",MessageBoxButton.OK);
        }

        private async void VerifyCodeButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string verifyCode = VerifyCodeTextBox.Text;
            (bool success, string result) = await RegistrationInterface.VerifyEmailCode(email,verifyCode);

            MessageBox.Show(result, "알림", MessageBoxButton.OK);
            StopTimer();
        }

        private DispatcherTimer _timer;
        private TimeSpan _timeLeft;

        private void StartTimer()
        {
            _timeLeft = TimeSpan.FromMinutes(3); // 3분
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timeLeft = _timeLeft.Subtract(TimeSpan.FromSeconds(1));
            TimerTextBlock.Text = $"{_timeLeft.Minutes:D2}:{_timeLeft.Seconds:D2}";

            if (_timeLeft <= TimeSpan.Zero)
            {
                _timer.Stop();
                TimerTextBlock.Text = "시간 초과";
            }
        }
        private void StopTimer()
        {
            _timer.Stop();
        }
    }
}
