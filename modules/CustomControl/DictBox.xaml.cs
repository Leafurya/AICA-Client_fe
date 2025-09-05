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
            //this.Loaded += DictBox_Loaded;
        }

        private void DictBox_Loaded(object sender, RoutedEventArgs e)
        {
            //if(this.DataContext is SharedViewModel vm)
            //{
            //    vm.IsLogin_AddWordButtonHandler += Vm_IsLogin_AddWordButtonHandler;
            //}
        }

        //private void Vm_IsLogin_AddWordButtonHandler(object? sender, EventArgs e)
        //{
        //    if (this.DataContext is SharedViewModel vm)
        //    {
        //        if (vm.IsLogin)
        //        {
        //            AddWord.IsEnabled
        //        }
        //    }
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RequestAddWord();
        }
        private async void RequestAddWord()
        {
            int textId = WordSearch.Interface.GetTextId();
            WordMeanings? wordMeanings = await VocabNote.Interface.RequestAddWord(textId);
            if (wordMeanings == null)
            {
                Debug.WriteLine("wordMeanings is null");
                return;
            }

            if (this.DataContext is SharedViewModel vm)
            {
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
                //if (vm.AicaList == null)
                //{
                //    vm.AicaList = VocabNote.Interface.GetAicaList();
                //}
                //else
                //{
                //    vm.AicaList.Add(wordMeanings);
                //}
            }
        }
    }
}
