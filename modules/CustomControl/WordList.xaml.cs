using CustomControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utility;
using Utility.Data.Word;

namespace CustomControl
{
    /// <summary>
    /// WordList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WordList : UserControl
    {
        private bool _suppressUnchecked = false;
        public WordList()
        {
            InitializeComponent();

            //InitList();
        }
        private void toggleButtonSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            IEnumerable<ToggleButton> toggleButtons = ControlUtil.FindVisualChildren<ToggleButton>(itemsControl).ToList();

            foreach (ToggleButton toggle in toggleButtons)
            {
                toggle.IsChecked = true; // 전체 On
            }
        }

        private void toggleButtonSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!_suppressUnchecked)
            {
                IEnumerable<ToggleButton> toggleButtons = ControlUtil.FindVisualChildren<ToggleButton>(itemsControl).ToList();

                foreach (ToggleButton toggle in toggleButtons)
                {
                    toggle.IsChecked = false; // 전체 Off
                }
            }
        }
        public void UncheckSelectAllTogglButton()
        {
            _suppressUnchecked = true;
            toggleButtonSelectAll.IsChecked = false;
            _suppressUnchecked = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<WordItem> items = ControlUtil.FindVisualChildren<WordItem>(itemsControl).ToList();
            List<int> removeTargetIdx = new List<int>();
            for (int i = 0; i < items.Count(); i++)
            {
                WordItem item = items[i];
                if (item.IsChecked())
                {
                    int wordId = item.GetWordId();
                    VocabNote.Interface.RequestDeleteWord(wordId, "mangoAccessToken");
                    removeTargetIdx.Add(i);
                }
            }

            if (this.DataContext is SharedViewModel vm)
            {
                //SentenceData data= vm.SentenceList
                foreach (int index in removeTargetIdx.OrderByDescending(i => i))
                {
                    vm.WordsList.RemoveAt(index);
                }
            }

            //foreach (SentenceItem item in items)
            //{
            //    int textId = item.GetTextId();
            //    SentenceManager.Interface.DeleteText(textId, "mangoAccessToken");
            //    Debug.WriteLine("textId " + textId);
            //    //item.IsChecked = false; // 전체 Off


            //}
            //if(this.DataContext is SharedViewModel vm)
            //{
            //}

        }

    }
}
