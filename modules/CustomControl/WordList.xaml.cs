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
        private WordItem? oldOpenedCard = null;
        public WordList()
        {
            InitializeComponent();
            this.Loaded += WordList_Loaded;
            //InitList();
        }

        private void WordList_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.DataContext is SharedViewModel vm)
            {
                vm.IsLogin_WordListHandler += Vm_IsLogin_WordListHandler;
            }
        }

        private async void Vm_IsLogin_WordListHandler(object? sender, EventArgs e)
        {
            if (this.DataContext is SharedViewModel vm)
            {
                //Debug.WriteLine("word list handler call");
                if (vm.IsLogin)
                {
                    await VocabNote.Interface.RequestVocabNote(-1);
                    vm.WordsList = VocabNote.Interface.GetWordList();
                    vm.AicaList = VocabNote.Interface.GetAicaList(WordSearch.Interface.GetTextId());
                    Notice.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Notice.Visibility = Visibility.Visible;
                }
            }
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // 단어 삭제
            List<WordItem> items = ControlUtil.FindVisualChildren<WordItem>(itemsControl).ToList();

            for (int i = 0; i < items.Count(); i++)
            {
                // 체크된 단어들만 가져옴
                WordItem item = items[i];
                if (item.IsChecked())
                {
                    int wordId = item.GetWordId();
                    Debug.WriteLine($"삭제한 단어 아이디: {wordId}");
                    await VocabNote.Interface.RequestDeleteWord(wordId); // 단어 삭제 요청
                }
            }

            if (this.DataContext is SharedViewModel vm)
            {
                // 업데이트된 단어장에서 AICA 단어장 새로 생성
                vm.WordsList = VocabNote.Interface.GetWordList();
                int nowTextId = WordSearch.Interface.GetTextId();
                if (nowTextId != -1)
                {
                    vm.AicaList = VocabNote.Interface.GetAicaList(nowTextId);
                }
            }
        }

        private void UserControl_OpenMeaningCard(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("in handler");
            WordItem? newOpenedCard = e.OriginalSource as WordItem;
            Debug.WriteLine(newOpenedCard);
            if (newOpenedCard == null)
            {
                return;
            }
            if (oldOpenedCard == null)
            {
                newOpenedCard.OpenCard();
                oldOpenedCard = newOpenedCard;
                return;
            }
            if (oldOpenedCard.GetWordId() == newOpenedCard.GetWordId())
            {
                newOpenedCard.CloseCard();
                oldOpenedCard = null;
                return;
            }
            newOpenedCard.OpenCard();
            oldOpenedCard.CloseCard();
            oldOpenedCard = newOpenedCard;
        }
    }
}
