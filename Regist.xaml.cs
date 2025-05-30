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
    }
}
