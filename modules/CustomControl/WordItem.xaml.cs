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
using static System.Net.Mime.MediaTypeNames;
using Utility;

namespace CustomControl
{
    /// <summary>
    /// ListItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WordItem : UserControl
    {
        public WordItem()
        {
            InitializeComponent();
        }
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            WordList? ucRoot = ControlUtil.FindAncestor<WordList>(this);
            ucRoot?.UncheckSelectAllTogglButton();
        }
        public int GetWordId()
        {
            return (int)Word.Tag;
        }
        public bool IsChecked()
        {
            return (bool)toggleBtn.IsChecked;
        }

        private void Word_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(MeaningsGrid.Visibility == Visibility.Visible)
            {
                MeaningsGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                MeaningsGrid.Visibility = Visibility.Visible;
            }
            
        }
    }
}
