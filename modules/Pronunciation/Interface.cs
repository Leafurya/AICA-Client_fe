
using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Linq;
using Utility.RequestConst;

namespace Pronunciation
{
    public class Interface
    {
        /*
         *  InitializeComponent();
            _ws.OnText += OnServerText;
            _ws.OnClosed += (code, reason) => Dispatcher.Invoke(() =>
            {
            TxtStatus.Text = $"Closed: {code} {reason}";
            BtnStart.IsEnabled = true;
            BtnStop.IsEnabled = false;
            });
         */
        private static WsClient _ws = new();
        private static AudioStreamer _audio = new();
        private static CancellationTokenSource? _cts;
        private static string wsEntryPoint = RequestConst.wsEntryPoint;

        private static bool isWSConnected  = false;
        private static bool isMicConnected = false;
        public static void Init(Action<string>? OnServerText)
        {
            _ws.OnText += OnServerText;
            _ws.OnClosed += (code, reason) =>
            {

            };
        }
        public static async Task Start(string word)
        {
            // 발음 비교 시작하면 팝업창 띄울 것
            try
            {
                await _ws.ConnectAsync(new Uri(wsEntryPoint));
                _cts = new CancellationTokenSource();
                var start = new { text = word };
                await _ws.SendTextAsync(JsonSerializer.Serialize(start), _cts.Token);
                isWSConnected = true;
            }
            catch (Exception ex)
            {
                isWSConnected = false;
                return;
            }

            try
            {
                // 실제 장치 샘플레이트 조회
                //int sampleRate = _audio.GetDeviceSampleRateOrDefault(16000);
                int sampleRate = 16000;
                Debug.WriteLine($"[C# Device SampleRate] {sampleRate} Hz");


                // 마이크 캡처 시작 (4096 샘플 당 전송)
                await _audio.StartAsync(sampleRate, async pcmChunk =>
                {
                    await _ws.SendBinaryAsync(pcmChunk, _cts!.Token);
                }, _cts.Token);
                isMicConnected=true;
            }
            catch (Exception ex)
            {
                isMicConnected = false;
                if (isWSConnected)
                {
                    try
                    {
                        await _ws.CloseAsync("mic start fail", _cts.Token);
                    }
                    catch { /*ignore*/ }
                }
                return;
            }
        }


        public static async void Stop()
        {
            // 발음 비교 끝나면 팝업을 끌 것
            try
            {
                //BtnStop.IsEnabled = false;
                _cts?.Cancel();
                _audio.Stop();
                await _ws.SendTextAsync(JsonSerializer.Serialize(new
                {
                    type="end"
                }));//"{\"type\":\"end\"}"
                await _ws.DisposeAsync();
                //TxtStatus.Text = "Stopped";
                //BtnStart.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //TxtStatus.Text = $"Stop error: {ex.Message}";
            }
        }


        //public static void OnServerText(string text)
        //{
        //    try
        //    {
        //        using var doc = JsonDocument.Parse(text);
        //        if (doc.RootElement.TryGetProperty("accuracyScore", out var acc))
        //        {
        //            double val = acc.GetDouble();
        //            if (val <= 1.0) val *= 100.0; // 서버가 0~1 범위일 경우 보정
        //            Debug.WriteLine(val);
        //            //vm의 값을 변경하여 정확도를 보여줄 것.
        //            //AccuracyBar.Value = Math.Clamp(val, 0, 100);
        //            //AccuracyText.Text = ((int)AccuracyBar.Value).ToString();
        //        }
        //    }
        //    catch (Exception ex) { }
        //}
        public static bool IsWebSocketConnected()
        {
            return isWSConnected;
        }
        public static bool IsMicConnected()
        {
            return isMicConnected;
        }
        public static string? GetMicDeviceName()
        {
            return _audio.GetDeviceName();
        }
    }
}
