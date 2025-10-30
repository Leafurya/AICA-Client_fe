using CustomControl.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Translate;
using Utility.Data.Json;
using Utility.Data.Word;
using Utility.TextSelector;
using static System.Net.Mime.MediaTypeNames;
//using static System.Net.Mime.MediaTypeNames;


namespace CustomControl
{
    /// <summary>
    /// SearchBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SearchBox : UserControl
    {
        int textId = -1;
        private bool _sentenceMode = false;
        public bool sentenceMode {
            get => _sentenceMode;
            set => _sentenceMode = value;
        }
        private bool searchMode = false;
        private ScrollViewer? scrollViewer=null;
        private bool flowTextChangeHandler = true;
        SharedViewModel vm;
        public SearchBox()
        {
            InitializeComponent();
            this.Loaded += OnSearchBoxLoaded;
            //if (this.DataContext is SharedViewModel vm)
            //{
            //    vm.PropertyChanged += UpdateText;
            //    this.vm = vm;
            //}
        }
        private void OnSearchBoxLoaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is SharedViewModel vm)
            {
                vm.PropertyChanged += UpdateText;
                this.vm = vm;

                // 초기값 반영
                UpdateText(vm, new PropertyChangedEventArgs(nameof(vm.NowText)));
            }
        }
        private void UpdateText(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.vm.NowText))
            {
                WordDataMap.Init();
                Dispatcher.Invoke(() =>
                {
                    flowTextChangeHandler = false;
                    FlowDocument doc = new FlowDocument();
                    doc.Blocks.Add(new Paragraph(new Run(this.vm.NowText)));
                    textBoxSearcher.Document = doc;
                    flowTextChangeHandler = true;
                });
            }
        }
        private async void Translate()
        {
            if (this.DataContext is SharedViewModel vm)
            {
                //Stopwatch stopwatch = new Stopwatch();

                //stopwatch.Start();

                //string result = await TranslatorText.ProcessTranslation();
                string result = ThirdParty.Interface.TranslateText(TranslatorText.ProcessTranslation());

                //stopwatch.Stop();

                //Debug.WriteLine($"번역 시간: {stopwatch.ElapsedMilliseconds} ms");
                
                vm.TranslateResult = result;
            }
        }
        private async void GetMeaning()
        {
            if (DataContext is SharedViewModel vm)
            {
                string result = await WordSearch.Interface.GetMeaning();
                vm.DictionaryMean = result;
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sentenceMode)
            {
                Translate();
            }
            else
            {
                if (WordSearch.Interface.HighlightPOS(textBoxSearcher))
                {
                    //GetMeaning();
                }
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(textBoxSearcher);

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                sentenceMode = true;
                SentenceManager.Interface.SelectRange(textBoxSearcher, mousePos, textId);
            }
            else
            {
                sentenceMode = false;
                Point targetPos=WordSearch.Interface.SelectRange(textBoxSearcher, mousePos, textId);
                WordData? data = WordDataMap.GetWordData((int)targetPos.X, (int)targetPos.Y);
                if (data != null)
                {
                    Debug.WriteLine(data.pos);
                    Debug.WriteLine(data.tag);
                    Debug.WriteLine(data.word);
                    //여기서 마우스 위치에 오버레이 띄우기
                    vm.OverlayPos = DataMaps.partOfSpeechMap[data.pos];
                    vm.OverlayTag = DataMaps.pennTreebankTagMap[data.tag];
                    vm.OverlayWord = data.word;
                    wordDataOverlay.HorizontalOffset = mousePos.X;
                    wordDataOverlay.VerticalOffset = mousePos.Y;
                    wordDataOverlay.IsOpen = true;
                }
                else
                {
                    //여기서 오버레이 지우기
                    wordDataOverlay.IsOpen = false;
                }
            }
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // RichTextBox 내부 ScrollViewer 가져오기
            if (scrollViewer == null)
            {
                scrollViewer = FindVisualChild<ScrollViewer>(textBoxSearcher);
            }
            //Debug.WriteLine("scroll"+ scrollViewer.ToString());
            if (scrollViewer != null)
            {
                if (e.Delta > 0)
                    scrollViewer.LineUp();     // 휠 ↑
                else
                    scrollViewer.LineDown();   // 휠 ↓
            }

            e.Handled = true; // 이벤트 버블링 방지 (필수)
        }


        private void ToggleBtnMode_Checked(object sender, RoutedEventArgs e)
        {
            searchMode = true;
            canvas.Visibility = Visibility.Visible;
            TextRange textRange = new TextRange(textBoxSearcher.Document.ContentStart, textBoxSearcher.Document.ContentEnd);
            string text = textRange.Text;
            text=text.Trim();

            //서버에서 받아온 데이터는 클라db에 없으니 해시 값도 없음.
            //그래서 한 번은 문장을 분석하고 db에 저장하는 과정을 거쳐야 함.
            if (!SentenceManager.Interface.IsExistText(text))
            {
                int selectedTextId = SentenceManager.Interface.GetSelectedTextId(); //리스트에서 클릭한 문장의 id
                Debug.WriteLine("selectedTextId " + selectedTextId);

                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();

                //textId = SentenceManager.Interface.PreProcess(text);
                textId=ThirdParty.Interface.AnalyzeSentence(text);

                stopwatch.Stop();

                Debug.WriteLine($"문장 분석 시간: {stopwatch.ElapsedMilliseconds} ms");

                if (selectedTextId != -1)
                {
                    SentenceManager.Interface.UpdateTextId(textId, selectedTextId); //db에 넣은 textid의 값을 기존의 것으로 변경
                    textId = selectedTextId;
                    SentenceManager.Interface.InitSelectedTextId();
                }
                else
                {
                    SentenceManager.Interface.SaveText(textId, text); //서버로 문장 데이터 전송

                    if (this.DataContext is SharedViewModel vm)
                    {
                        Debug.WriteLine("input update");
                        vm.NowText = text;
                        vm.SentenceList.Add(SentenceManager.Interface.AddText(text, textId));
                    }
                }
            }
            else
            {
                //SentenceManager.Interface.GetSelectedTextId()를 하면 안되는 이유:
                //프로그램이 해당 문장의 해시 값이 이미 가지고 있음
                //문장을 수정했다가 원상 복구 하면 GetSelectedTextId의 출력 값이 달라짐.
                //그럼 해시 값은 같은 데 GetSelectedTextId가 -1이 되어 textid의 값이 -1이 됨
                //이를 방지하고자 해당 텍스트의 id를 직접 가져옴
                textId = SentenceManager.Interface.GetTextId(text);
                Debug.WriteLine("selected id " + textId);
            }
            WordSearch.Interface.SetTextId(textId);
            Debug.WriteLine(textId);
        }

        private void ToggleBtnMode_Unchecked(object sender, RoutedEventArgs e)
        {
            searchMode = false;
            canvas.Visibility = Visibility.Collapsed;
        }
        public static T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T correctlyTyped)
                    return correctlyTyped;

                var result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        private void btnSelectImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "이미지 파일 (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";
            if (dialog.ShowDialog() == true)
            {
                string selectedImage = dialog.FileName;

                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();

                string result=ThirdParty.Interface.ExtractText(selectedImage);
                //string result = SentenceManager.Interface.GetStringFromImg(selectedImage);

                stopwatch.Stop();

                Debug.WriteLine($"문장 추출 시간: {stopwatch.ElapsedMilliseconds} ms");

                
                if(this.DataContext is SharedViewModel vm)
                {
                    vm.NowText = result;
                }
                Debug.WriteLine(result);
            }
        }

        private void textBoxSearcher_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (flowTextChangeHandler)
            {
                SentenceManager.Interface.InitSelectedTextId();
            }
        }
    }
}
