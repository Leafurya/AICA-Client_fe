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
using Utility.Data.User;
using Utility.Data.UserData;
using Utility.Data.Word;
using Utility.DataBase;

namespace CustomControl.ViewModel
{
    public class SharedViewModel : INotifyPropertyChanged
    {
        //public ObservableCollection<SentenceData> SentenceList
        //{
        //    get; set;
        //}
        //private UserSettingData _settingData;
        public UserSettingData SettingData { get; set; } = new UserSettingData();
        private ObservableCollection<SentenceData> _sentenceList;
        public ObservableCollection<SentenceData> SentenceList
        {
            get => _sentenceList;
            set
            {
                if (_sentenceList != value)
                {
                    _sentenceList = value;
                    OnPropertyChanged(nameof(SentenceList));
                }
            }
        }

        private ObservableCollection<VocabItem>? _wordsList;
        public ObservableCollection<VocabItem>? WordsList
        {
            get => _wordsList;
            set
            {
                if (_wordsList != value)
                {
                    _wordsList = value;
                    OnPropertyChanged(nameof(WordsList));
                }
            }
        }
        private ObservableCollection<VocabItem>? _aicaList;
        public ObservableCollection<VocabItem>? AicaList
        {
            get => _aicaList;
            set
            {
                if (_aicaList != value)
                {
                    _aicaList = value;
                    OnPropertyChanged(nameof(AicaList));
                }
            }
        }
        //public ObservableCollection<WordMeanings>? WordsList
        //{
        //    get; set;
        //}

        private string _userinfoID;
        public string UserInfoID
        {
            get => _userinfoID;
            set
            {
                if (_userinfoID != value)
                {
                    _userinfoID = value;
                    OnPropertyChanged(nameof(UserInfoID));
                }
            }
        }
        private string _userinfoEmail;
        public string UserInfoEmail
        {
            get => _userinfoEmail;
            set
            {
                if (_userinfoEmail != value)
                {
                    _userinfoEmail = value;
                    OnPropertyChanged(nameof(UserInfoEmail));
                }
            }
        }
        private string _userinfoAlias;
        public string UserInfoAlias
        {
            get => _userinfoAlias;
            set
            {
                if (_userinfoAlias != value)
                {
                    _userinfoAlias = value;
                    OnPropertyChanged(nameof(UserInfoAlias));
                }
            }
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

        public event EventHandler? IsLogin_LoginButtonHandler;
        //public event EventHandler? IsLogin_AddWordButtonHandler;
        public event EventHandler? IsLogin_LoadSentenceButtonHandler;
        public event EventHandler? IsLogin_WordListHandler;
        public event EventHandler? IsLogin_AicaListHandler;
        private bool _isLogin;
        public bool IsLogin
        {
            get => _isLogin;
            set
            {
                if (_isLogin != value)
                {
                    _isLogin = value;
                    OnPropertyChanged(nameof(IsLogin));
                    IsLogin_LoginButtonHandler?.Invoke(this, new EventArgs());
                    //IsLogin_AddWordButtonHandler?.Invoke(this, new EventArgs());
                    IsLogin_LoadSentenceButtonHandler?.Invoke(this, new EventArgs());
                    IsLogin_WordListHandler?.Invoke(this, new EventArgs());
                    IsLogin_AicaListHandler?.Invoke(this,new EventArgs());
                }
            }
        }

        //private User userData {  get; set; }

        public SharedViewModel()
        {

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                SentenceList = SentenceManager.Interface.GetTextList(); // 런타임 전용
                //SettingData = new UserSettingData();

                //InitWordsList();
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
                //userData = new User { alias = "배재", id = "metalhyun", pwd = "123", email = "metal@hyun.com", token = null };

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
            await VocabNote.Interface.RequestVocabNote(-1);
            WordsList = VocabNote.Interface.GetWordList();
        }
    }
}
