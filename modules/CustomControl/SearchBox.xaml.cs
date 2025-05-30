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
                Debug.WriteLine("구독 완료");
                this.vm = vm;

                // 초기값 반영
                UpdateText(vm, new PropertyChangedEventArgs(nameof(vm.NowText)));
            }
        }
        private void UpdateText(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.vm.NowText))
            {
                Debug.WriteLine("text is update "+ this.vm.NowText);
                Dispatcher.Invoke(() =>
                {
                    FlowDocument doc = new FlowDocument();
                    doc.Blocks.Add(new Paragraph(new Run(this.vm.NowText)));
                    textBoxSearcher.Document = doc;
                    Debug.WriteLine("invoke");
                });
            }
        }
        private async void Translate()
        {
            if (this.DataContext is SharedViewModel vm)
            {
                string result=await TranslatorText.ProcessTranslation();
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
                WordSearch.Interface.HighlightPOS(textBoxSearcher);
                GetMeaning();
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
                WordSearch.Interface.SelectRange(textBoxSearcher, mousePos, textId);
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

            if (!SentenceManager.Interface.IsExistText(text))
            {
                textId = SentenceManager.Interface.PreProcess(text);
                SentenceManager.Interface.SaveText(textId, "mangoAccessToken", text);

                if (this.DataContext is SharedViewModel vm)
                {
                    Debug.WriteLine("input update");
                    vm.NowText = text;
                    vm.SentenceList.Add(SentenceManager.Interface.AddText(text, textId));
                }

                WordSearch.Interface.SetTextId(textId);
            }
            else
            {
                textId= WordSearch.Interface.GetTextId();
            }
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
                string result = SentenceManager.Interface.GetStringFromImg(selectedImage);
                if(this.DataContext is SharedViewModel vm)
                {
                    vm.NowText = result;
                }
                Debug.WriteLine(result);
            }
        }

        private void textBoxSearcher_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}
