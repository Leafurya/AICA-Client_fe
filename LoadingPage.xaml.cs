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
using System.Windows.Threading;

namespace AICA_Client
{
    
    /// <summary>
    /// LoadingPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoadingPage : Page
    {
        private readonly string[] _frames = { "로딩 중.  ", "로딩 중.. ", "로딩 중..." };
        private readonly DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) };
        private int _idx = 0;
        public LoadingPage(string[]? message)
        {
            InitializeComponent();
            if (message != null)
            {
                this._frames = message;
            }
            _timer.Tick += (s, e) =>
            {
                content.Text = _frames[_idx++ % _frames.Length];
            };
        }
        public LoadingPage()
        {
            InitializeComponent();
            _timer.Tick += (s, e) =>
            {
                content.Text = _frames[_idx++ % _frames.Length];
            };
        }
        public void Start()
        {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
            _idx = 0;
        }
    }
}
