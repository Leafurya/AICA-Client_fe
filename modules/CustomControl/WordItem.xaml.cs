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
using static System.Net.Mime.MediaTypeNames;
using Utility;
using System.Diagnostics;

namespace CustomControl
{
    /// <summary>
    /// ListItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WordItem : UserControl
    {
        public static readonly RoutedEvent OpenPronunciationFrameEvent=EventManager.RegisterRoutedEvent(
            nameof(OpenPronunciationFrame),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(WordItem));

        public event RoutedEventHandler OpenPronunciationFrame
        {
            add => AddHandler(OpenPronunciationFrameEvent, value);
            remove => RemoveHandler(OpenPronunciationFrameEvent, value);
        }

        public static readonly RoutedEvent OpenMeaningCardEvent=EventManager.RegisterRoutedEvent(
            nameof(OpenMeaningCard),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(WordItem));

        public event RoutedEventHandler OpenMeaningCard
        {
            add => AddHandler(OpenMeaningCardEvent, value);
            remove => RemoveHandler(OpenMeaningCardEvent, value);
        }

        public WordItem()
        {
            InitializeComponent();
        }
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            WordList? ucRoot = ControlUtil.FindAncestor<WordList>(this);
            ucRoot?.UncheckSelectAllTogglButton();
        }
        public int GetWordId()
        {
            return (int)Word.Tag;
        }
        public string GetWord()
        {
            return Word.Text;
        }
        public bool IsChecked()
        {
            return (bool)toggleBtn.IsChecked;
        }

        private void Word_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            //if(MeaningsGrid.Visibility == Visibility.Visible)
            //{
            //    MeaningsGrid.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    MeaningsGrid.Visibility = Visibility.Visible;
            //}
            Debug.WriteLine("word click");
            RaiseEvent(new RoutedEventArgs(OpenMeaningCardEvent, this));
        }
        public void CloseCard()
        {
            MeaningsGrid.Visibility = Visibility.Collapsed;
        }
        public void OpenCard()
        {
            MeaningsGrid.Visibility = Visibility.Visible;
        }

        private void btnPronuncTest_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OpenPronunciationFrameEvent,this));
        }
    }
}
