using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using UserAccountManager.Helpers;


namespace UserAccountManager.Services
{
    public class EmailVerificationHandler
    {
        public static string SentCode { get; private set; }
        private static DispatcherTimer _timer;
        private static int _timeLeft;

        public static bool IsVerified { get; set; } = false;

        public static async Task HandleSendCodeAsync(string email, Action<string> notify, Action<string> updateTimerUI, Action<string> changeButtonText)
        {
            var result = UserRegistrationValidator.ValidateEmail(email);
            if (!result.IsValid)
            {
                notify(result.ErrorMessage);
                return;
            }

            var (success, code, message) = await EmailService.RequestAuthCodeFromServerAsync(email);
            if (!success)
            {
                notify(message);
                return;
            }

            SentCode = code;
            IsVerified = false;
            notify("인증번호가 전송되었습니다.");
            changeButtonText("재전송");

            StartTimer(updateTimerUI);
        }

        public static async Task HandleVerifyCodeAsync(string email, string userInputCode, Action<string> notify)
        {
            var (success, message) = await EmailService.VerifyCodeWithServerAsync(email, userInputCode);

            if (success)
                notify("이메일 인증 완료!");
            else
                notify(message);
        }
        private static void StartTimer(Action<string> updateTimerUI)
        {
            _timeLeft = 180; // 3분

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timer.Tick += (s, e) =>
            {
                _timeLeft--;
                int min = _timeLeft / 60;
                int sec = _timeLeft % 60;
                updateTimerUI($"{min:D2}:{sec:D2}");

                if (_timeLeft <= 0)
                {
                    _timer.Stop();
                    updateTimerUI("시간 만료");

                    IsVerified = false;
                }
            };

            _timer.Start();
        }

        public static void StopTimer() => _timer?.Stop();


    }
}
