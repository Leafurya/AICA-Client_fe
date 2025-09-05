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
using Utility;
using Utility.Data.Word;
using static System.Net.Mime.MediaTypeNames;

namespace CustomControl
{
    /// <summary>
    /// AicaItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AicaItem : UserControl
    {
        public AicaItem()
        {
            InitializeComponent();
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            AicaList? ucRoot = ControlUtil.FindAncestor<AicaList>(this);
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
            ItemsControl? itemsControl = ControlUtil.FindParent<ItemsControl>(this);
            if (itemsControl?.DataContext is SharedViewModel sharedVM)
            {
                if (sharedVM.IsLogin)
                {
                    //Debug.WriteLine(this.DataContext is VocabItem);
                    if(this.DataContext is VocabItem vm)
                    {
                        sharedVM.DictionaryMean = vm.ToString();
                    }
                }
            }
        }

        private void toggleBtn_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
