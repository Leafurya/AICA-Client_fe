using CustomControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
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

namespace AICA_Client
{
    /// <summary>
    /// PronunciationTest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PronunciationTest : Page
    {
        private string word;
        private MainWindow mainWnd;
        private SharedViewModel sharedVM;
        public PronunciationTest(string word,MainWindow mainWnd)
        {
            InitializeComponent();
            this.word = word;
            this.mainWnd = mainWnd;
            this.Loaded += PronunciationTest_Loaded;
        }

        private async void PronunciationTest_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("현재 DataContext: " + (this.DataContext?.GetType().FullName ?? "null"));

            if (this.DataContext is SharedViewModel vm)
            {
                sharedVM = vm;
            }
            sharedVM.PronuncScore = 0;
            Debug.WriteLine(this.sharedVM.ToString());
            txtWord.Text = word;
            Pronunciation.Interface.Init(OnServerText);
            await Pronunciation.Interface.Start(word);
            // 마이크 연결 됐는지
            sharedVM.IsMicConnected = Pronunciation.Interface.IsMicConnected();
            sharedVM.MicDeviceName = Pronunciation.Interface.GetMicDeviceName();
            // 서버랑 연결 됐는지 확인하고 출력할 것
            sharedVM.IsWSConnected = Pronunciation.Interface.IsMicConnected();
            Debug.WriteLine(sharedVM.IsMicConnected);
            Debug.WriteLine(sharedVM.MicDeviceName);
            Debug.WriteLine(sharedVM.IsWSConnected);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Pronunciation.Interface.Stop();
            this.mainWnd.CloseFrameContainer();
        }
        private void OnServerText(string text)
        {
            try
            {
                Debug.WriteLine(text);
                using var doc = JsonDocument.Parse(text);
                if (doc.RootElement.TryGetProperty("accuracyScore", out var acc))
                {
                    double val = acc.GetDouble();
                    if (val <= 1.0) val *= 100.0; // 서버가 0~1 범위일 경우 보정
                    Debug.WriteLine(val);
                    sharedVM.PronuncScore = val;
                    //vm의 값을 변경하여 정확도를 보여줄 것.
                    //AccuracyBar.Value = Math.Clamp(val, 0, 100);
                    //AccuracyText.Text = ((int)AccuracyBar.Value).ToString();
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
