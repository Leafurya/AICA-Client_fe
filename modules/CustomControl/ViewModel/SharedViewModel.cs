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
using System.Windows.Data;
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

        // 발음 비교용
        private bool _isMicConnected;
        public bool IsMicConnected
        {
            get => _isMicConnected;
            set
            {
                if (_isMicConnected != value)
                {
                    _isMicConnected = value;
                    OnPropertyChanged(nameof(IsMicConnected));
                }
            }
        }
        private bool _isWSConnected;
        public bool IsWSConnected
        {
            get => _isWSConnected;
            set
            {
                if (_isWSConnected != value)
                {
                    _isWSConnected = value;
                    OnPropertyChanged(nameof(IsWSConnected));
                }
            }
        }
        private string? _micDeviceName;
        public string? MicDeviceName
        {
            get => _micDeviceName;
            set
            {
                if (_micDeviceName != value)
                {
                    _micDeviceName = value;
                    OnPropertyChanged(nameof(MicDeviceName));
                }
            }
        }
        private double _pronuncScore;
        public double PronuncScore
        {
            get => _pronuncScore;
            set
            {
                if (_pronuncScore != value)
                {
                    _pronuncScore = value;
                    OnPropertyChanged(nameof(PronuncScore));
                }
            }
        }

        // 사전
        private List<Meaning> _dictionaryMeans;
        public List<Meaning> DictionaryMeans
        {
            get => _dictionaryMeans;
            set
            {
                if (_dictionaryMeans != value)
                {
                    _dictionaryMeans = value;
                    OnPropertyChanged(nameof(DictionaryMeans));
                }
            }
        }

        private string _dictword;
        public string DictWord
        {
            get => _dictword;
            set
            {
                if (_dictword != value)
                {
                    _dictword = value;
                    OnPropertyChanged(nameof(DictWord));
                }
            }
        }

        // 사용자 설정
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

        // 단어장
        private ObservableCollection<VocabItem>? _wordsList;
        public ObservableCollection<VocabItem>? WordsList
        {
            get => _wordsList;
            set
            {
                if (_wordsList != value)
                {
                    _wordsList = value ?? new ObservableCollection<VocabItem>();

                    HookFilteredWordListView();
                    OnPropertyChanged(nameof(WordsList));
                    OnPropertyChanged(nameof(FilteredWordList));
                }
            }
        }

        // AICA 단어장
        private ObservableCollection<VocabItem>? _aicaList;
        public ObservableCollection<VocabItem>? AicaList
        {
            get => _aicaList;
            set
            {
                if (_aicaList != value)
                {
                    _aicaList = value ?? new ObservableCollection<VocabItem>();
                    HookFilteredAICAListView();
                    OnPropertyChanged(nameof(AicaList));
                    OnPropertyChanged(nameof(FilteredAICAList));
                }
            }
        }
        // 오버레이 관련
        private string _overlayWord;
        public string OverlayWord
        {
            get => _overlayWord;
            set
            {
                if (_overlayWord != value)
                {
                    _overlayWord = value;
                    OnPropertyChanged(nameof(OverlayWord));
                }
            }
        }
        private string _overlayPos;
        public string OverlayPos
        {
            get => _overlayPos;
            set
            {
                if (_overlayPos != value)
                {
                    _overlayPos = value;
                    OnPropertyChanged(nameof(OverlayPos));
                }
            }
        }
        private string _overlayTag;
        public string OverlayTag
        {
            get => _overlayTag;
            set
            {
                if (_overlayTag != value)
                {
                    _overlayTag = value;
                    OnPropertyChanged(nameof(OverlayTag));
                }
            }
        }


        // 회원 정보 관련
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

        // 검색기 관련
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

        // 헨들러들
        public event EventHandler? IsLogin_LoginButtonHandler;
        public event EventHandler? IsLogin_LoadSentenceButtonHandler;
        public event EventHandler? IsLogin_WordListHandler;
        public event EventHandler? IsLogin_AicaListHandler;

        // 로그인 됐는지
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
                    IsLogin_LoadSentenceButtonHandler?.Invoke(this, new EventArgs());
                    IsLogin_WordListHandler?.Invoke(this, new EventArgs());
                    IsLogin_AicaListHandler?.Invoke(this,new EventArgs());
                }
            }
        }


        // 단어 검색
        private ICollectionView _filteredWordList;
        public ICollectionView FilteredWordList
        {
            get => _filteredWordList;
            private set
            {
                if (_filteredWordList == value) return;
                _filteredWordList = value;
                OnPropertyChanged(nameof(FilteredWordList));
            }
        }

        private ICollectionView _filteredAICAList;
        public ICollectionView FilteredAICAList
        {
            get => _filteredAICAList;
            private set
            {
                if (_filteredAICAList == value) return;
                _filteredAICAList = value;
                OnPropertyChanged(nameof(FilteredAICAList));
            }
        }

        // 단어장 검색어
        private string _wordSearchQuery = "";
        public string WordSearchQuery
        {
            get => _wordSearchQuery;
            set
            {
                if (_wordSearchQuery == value) return;
                _wordSearchQuery = value;
                OnPropertyChanged(nameof(IsSearchModeToggleOn));
                FilteredWordList.Refresh();
            }
        }

        // AICA 단어장 검색어
        private string _aicaSearchQuery = "";
        public string AicaSearchQuery
        {
            get => _aicaSearchQuery;
            set
            {
                if (_aicaSearchQuery == value) return;
                _aicaSearchQuery = value;
                OnPropertyChanged(nameof(IsSearchModeToggleOn));
                FilteredAICAList.Refresh();
            }
        }
        private bool FilterWord(object? item)
        {
            Debug.WriteLine("FilterWord item1 ", item);
            if (item is not VocabItem i)
            {
                return false;
            }
            Debug.WriteLine("FilterWord item2 ", i.word);
            if (string.IsNullOrWhiteSpace(WordSearchQuery))
            {
                return true; // 검색어 없으면 전부 표시
            }
            Debug.WriteLine("FilterWord item3 ", i.word);
            return i.word.Contains(WordSearchQuery, StringComparison.OrdinalIgnoreCase);
        }
        private bool FilterAICA(object? item)
        {
            Debug.WriteLine("FilterAICA item1 "+ item);
            if (item is not VocabItem i)
            {
                return false;
            }
            Debug.WriteLine("FilterAICA item2 "+ i.word);
            if (string.IsNullOrWhiteSpace(AicaSearchQuery))
            {
                return true; // 검색어 없으면 전부 표시
            }
            Debug.WriteLine("FilterAICA item3 "+ i.word);
            return i.word.Contains(AicaSearchQuery, StringComparison.OrdinalIgnoreCase);
        }

        public SharedViewModel()
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                SentenceList = SentenceManager.Interface.GetTextList(); // 런타임 전용

                NowText = "";
                WordsList = new ObservableCollection<VocabItem>();
                AicaList = new ObservableCollection<VocabItem>();

                HookFilteredWordListView();
                HookFilteredAICAListView();
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
            }
        }

        

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void HookFilteredWordListView()
        {
            var view = CollectionViewSource.GetDefaultView(WordsList);
            view.Filter = FilterWord;
            FilteredWordList = view;
            FilteredWordList.Refresh();
        }
        private void HookFilteredAICAListView()
        {
            var view = CollectionViewSource.GetDefaultView(AicaList);
            view.Filter = FilterAICA;
            FilteredAICAList = view;
            FilteredAICAList.Refresh();
        }
    }
}
