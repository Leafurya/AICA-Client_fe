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
        public int GetWordId()
        {
            return (int)Word.Tag;
        }

        private void Word_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ItemsControl? itemsControl = ControlUtil.FindParent<ItemsControl>(this);
            if (itemsControl?.DataContext is SharedViewModel sharedVM)
            {
                if (sharedVM.IsLogin)
                {
                    if(this.DataContext is VocabItem vm)
                    {
                        sharedVM.DictionaryMeans = vm.meanings;
                        sharedVM.DictWord = vm.word;
                    }
                }
            }
        }
    }
}
