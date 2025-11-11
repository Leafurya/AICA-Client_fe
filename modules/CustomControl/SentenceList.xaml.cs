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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utility;
using Utility.Data.Sentence;
using static System.Net.Mime.MediaTypeNames;

namespace CustomControl
{
    /// <summary>
    /// SentenceList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SentenceList : UserControl
    {
        private bool _suppressUnchecked = false;
        public SentenceList()
        {
            InitializeComponent();
            this.Loaded += SentenceList_Loaded;
        }

        private void SentenceList_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.DataContext is SharedViewModel vm)
            {
                //문장리스트 싱크 맞추기
                //문장 받고 로컬에 있는거랑 비교해서 다른 거 있으면 전송하기
                vm.IsLogin_LoadSentenceButtonHandler += Vm_IsLogin_LoadSentenceButtonHandler;
            }
        }

        private async void Vm_IsLogin_LoadSentenceButtonHandler(object? sender, EventArgs e)
        {
            if (this.DataContext is SharedViewModel vm)
            {
                //문장리스트 싱크 맞추기
                //문장 받고 로컬에 있는거랑 비교해서 다른 거 있으면 전송하기
                if (vm.IsLogin)
                {
                    await SentenceManager.Interface.RequestTextList();
                    vm.SentenceList = SentenceManager.Interface.GetTextList();
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
            List<SentenceItem> items = ControlUtil.FindVisualChildren<SentenceItem>(itemsControl).ToList();
            List<int> removeTargetIdx= new List<int>();
            int nowTextId = WordSearch.Interface.GetTextId();

            
            for (int i = 0; i < items.Count(); i++)
            {
                SentenceItem item = items[i];
                if (item.IsChecked())
                {
                    int textId = item.GetTextId();
                    await SentenceManager.Interface.DeleteText(textId); // 문장 삭제
                    if (this.DataContext is SharedViewModel sharedVm)
                    {
                        // 관련 단어 삭제 

                        if (sharedVm.IsLogin)
                        {
                            VocabNote.Interface.Clear();
                            await VocabNote.Interface.RequestVocabNote(-1);
                            sharedVm.WordsList = VocabNote.Interface.GetWordList();
                        }
                        if (nowTextId != -1)
                        {
                            sharedVm.AicaList = VocabNote.Interface.GetAicaList(nowTextId);
                        }

                        if (nowTextId == textId) // 지금 떠있는 문장을 삭제했다면
                        {
                            WordSearch.Interface.SetTextId(-1);
                            sharedVm.NowText = "";
                        }
                    }
                    removeTargetIdx.Add(i);
                }
            }

            if (this.DataContext is SharedViewModel vm)
            {
                foreach (int index in removeTargetIdx.OrderByDescending(i => i))
                {
                    vm.SentenceList.RemoveAt(index);
                }
            }
        }
    }
}
