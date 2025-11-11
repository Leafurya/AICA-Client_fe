using CustomControl.ViewModel;
using System;
using System.Collections.Generic;
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

namespace CustomControl
{
    /// <summary>
    /// AicaList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AicaList : UserControl
    {
        private bool _suppressUnchecked = false;
        
        public AicaList()
        {
            InitializeComponent();
            this.Loaded += AicaList_Loaded;
        }

        private void AicaList_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.DataContext is SharedViewModel vm)
            {
                vm.IsLogin_AicaListHandler += Vm_IsLogin_AicaListHandler;
            }
        }

        private void Vm_IsLogin_AicaListHandler(object? sender, EventArgs e)
        {
            if (this.DataContext is SharedViewModel vm)
            {
                if (vm.IsLogin)
                {
                    int textId = WordSearch.Interface.GetTextId();
                    if (textId != -1)
                    {
                        vm.AicaList = VocabNote.Interface.GetAicaList(textId);
                    }
                    Notice.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Notice.Visibility = Visibility.Visible;
                }
            }
        }        
    }
}
