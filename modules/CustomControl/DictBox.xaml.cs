using CustomControl.ViewModel;
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
            WordMeanings wordMeanings = await VocabNote.Interface.RequestAddWord(textId, "mangoAccessToken");

            if(this.DataContext is SharedViewModel vm)
            {
                vm.WordsList.Add(wordMeanings);
            }
        }
    }
}
