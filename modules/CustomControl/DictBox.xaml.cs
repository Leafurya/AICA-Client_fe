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
using Utility.Data.Word;

namespace CustomControl
{
    /// <summary>
    /// DictBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DictBox : UserControl
    {
        public DictBox()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RequestAddWord();
        }
        private async void RequestAddWord()
        {
            int textId = WordSearch.Interface.GetTextId();
            Debug.WriteLine("textId "+textId);
            if (this.DataContext is not SharedViewModel vm)
            {
                Debug.WriteLine("this.DataContext is not SharedViewModel vm @ DictBox.xaml.cs");
                return;
            }
            if (!vm.IsLogin)
            {
                Debug.WriteLine("로그인 필요 @ DictBox.xaml.cs");
                return;
            }
            if (textId == -1)
            {
                Debug.WriteLine("textId == -1 @ DictBox.xaml.cs");
                return;
            }
            WordMeanings? wordMeanings = await VocabNote.Interface.RequestAddWord(textId);
            if (wordMeanings == null)
            {
                Debug.WriteLine("wordMeanings is null @ DictBox.xaml.cs");
                return;
            }
            if (vm.WordsList == null)
            {
                vm.WordsList = VocabNote.Interface.GetWordList();
            }
            else
            {
                (bool vocabModi,bool aicaModi)=VocabNote.Interface.GetNoteModified();
                VocabItem item = (VocabItem)wordMeanings;
                item.sentenceId = textId;
                if (vocabModi)
                {
                    vm.WordsList.Add(item);
                }
                if (aicaModi)
                {
                    vm.AicaList.Add(item);
                }
            }
        }
    }
}
