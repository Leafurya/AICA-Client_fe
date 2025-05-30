using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Utility.Data.Sentence;

namespace CustomControl.ViewModel
{
    public class SentenceViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SentenceData> SentenceList
        {
            get; set;
        }

        public SentenceViewModel()
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                SentenceList = SentenceManager.Interface.GetTextList(); // 런타임 전용
            }
            else
            {
                // 디자인 미리보기용 더미 데이터
                SentenceList = new ObservableCollection<SentenceData>
                {
                    new SentenceData { sentence = "디자인 타임 문장 1" ,sentenceId=1},
                    new SentenceData { sentence = "디자인 타임 문장 2" ,sentenceId=2}
                };
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
