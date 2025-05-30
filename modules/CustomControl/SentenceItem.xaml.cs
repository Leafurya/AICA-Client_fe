using CustomControl.ViewModel;
using SentenceManager;
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
using Utility.Data.Sentence;


namespace CustomControl
{
    /// <summary>
    /// SentenceItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SentenceItem : UserControl
    {
        public SentenceItem()
        {
            InitializeComponent();
        }

        private void Text_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            /*richtextbox에 vm 반영하기*/
            if (sender is TextBlock clickedTextBlock)
            {
                Debug.WriteLine("tag: " + clickedTextBlock.Tag);
                WordSearch.Interface.SetTextId((int)clickedTextBlock.Tag);
                ItemsControl? itemsControl =ControlUtil.FindParent<ItemsControl>(this);
                if (itemsControl?.DataContext is SharedViewModel sharedVM)
                {
                    sharedVM.NowText = clickedTextBlock.Text;
                    sharedVM.IsSearchModeToggleOn = false;
                }
            }
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            SentenceList? ucRoot = ControlUtil.FindAncestor<SentenceList>(this);
            ucRoot?.UncheckSelectAllTogglButton();
        }
        public int GetTextId()
        {
            return (int)Text.Tag;
        }
        public bool IsChecked()
        {
            return (bool)toggleBtn.IsChecked;
        }
    }
}
