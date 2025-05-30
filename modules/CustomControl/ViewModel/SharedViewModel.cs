using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Utility.Data.Sentence;
using Utility.Data.Word;

namespace CustomControl.ViewModel
{
    public class SharedViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SentenceData> SentenceList
        {
            get; set;
        }
        public ObservableCollection<WordMeanings> WordsList
        {
            get; set;
        }



        private string _dictionaryMean;
        public string DictionaryMean
        {
            get => _dictionaryMean;
            set
            {
                if (_dictionaryMean != value)
                {
                    _dictionaryMean = value;
                    OnPropertyChanged(nameof(DictionaryMean));
                }
            }
        }


        private string _nowText;
        public string NowText
        {
            get => _nowText;
            set
            {
                if (_nowText != value)
                {
                    _nowText = value;
                    OnPropertyChanged(nameof(NowText));
                }
            }
        }
        private string _translateResult;
        public string TranslateResult
        {
            get => _translateResult;
            set
            {
                if (_translateResult != value)
                {
                    _translateResult = value;
                    OnPropertyChanged(nameof(TranslateResult));
                }
            }
        }
        private bool _isSearchModeToggleOn;
        public bool IsSearchModeToggleOn
        {
            get => _isSearchModeToggleOn;
            set
            {
                if (_isSearchModeToggleOn != value)
                {
                    _isSearchModeToggleOn = value;
                    OnPropertyChanged(nameof(IsSearchModeToggleOn));
                }
            }
        }

        public SharedViewModel()
        {

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                SentenceList = SentenceManager.Interface.GetTextList(); // 런타임 전용
                InitWordsList();
                NowText = "";
            }
            else
            {
                // 디자인 미리보기용 더미 데이터
                NowText = "test text";
                SentenceList = new ObservableCollection<SentenceData>
                {
                    new SentenceData { sentence = "디자인 타임 문장 1" ,sentenceId=1},
                    new SentenceData { sentence = "디자인 타임 문장 2" ,sentenceId=2}
                };

                //WordsList = new ObservableCollection<JustWord>
                //{
                //    new JustWord { word = "apple" ,wordId=1},
                //    new JustWord { word = "banana" ,wordId=2}
                //};
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private async void InitWordsList()
        {
            await VocabNote.Interface.RequestVocabNote(-1, "accessTokenMango");
            WordsList = VocabNote.Interface.GetWordList();
        }



    }
}
