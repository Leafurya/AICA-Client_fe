using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//using WordSearch;

namespace AICA_Client;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        richTextBox.Document.Blocks.Add(new Paragraph(new Run("Thank you for joining us. I'm Yoon Jung-min.\nThe Constitutional Court is yet to disclose a date for its ruling on the impeachment of President Yoon Suk Yeol.\nWhile many had initially believed a verdict would be delivered on Friday, some are now speculating the ruling may take place later than that.\nShin Ha-young explains.\n \nIt's been two weeks since the final hearing in President Yoon Suk Yeol's impeachment trial, but the Constitutional Court has not yet announced a date for its ruling.\nGiven that previous presidential impeachment rulings were made about two weeks after the final hearing, many expected the ruling to be this Friday.\nHowever, since the court decided to deliver impeachment verdicts on the chief state auditor and three top prosecutors on Thursday, expectations are rising that the ruling on Yoon could be delayed until next week.\nThere is no precedent for the court delivering major rulings on consecutive days.\n\nAhead of Yoon's verdict, police are considering banning the release of stored firearms to prevent potential attacks.\nThe National Police Agency said it's reviewing refusing to release firearms stored at police stations used to kill dangerous wild animals.\nUnder the current law, licensed gun owners must store their firearms at police stations and only take them out to hunt down wild boars or birds.\nOn the day of the ruling, police will designate parts of Seoul's Jongno-gu and Jung-gu districts near the Constitutional Court as special crime prevention zones to maintain safety and manage crowds.\nVehicle barriers will be set up within 100 meters of the Court.\n\nDuring a Cabinet meeting on Tuesday, Acting President Choi Sang-mok expressed concern about possible national division and conflict over the presidential impeachment ruling.\n\n\"The government will respond firmly according to the law with zero tolerance for any illegal or violent protests as well as any acts that challenge public authority.\"\n \n\nMeanwhile, the main opposition Democratic Party began a sit-in protest at Gwanghwamun Square on Tuesday, calling for Yoon's removal, while the ruling People Power Party decided not to stage any demonstrations to pressure the Court.\n\nShin Ha-young, Arirang News.")));
    }

    private void Canvas_MouseMove(object sender, MouseEventArgs e)
    {
        Point mousePos = e.GetPosition(richTextBox);
        WordSearch.Interface.SelectRange(richTextBox, mousePos);
    }
    private async void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        WordSearch.Interface.HighlightPOS(richTextBox);
        
        //Debug.WriteLine("click");

        // 1. RichTextBox 기준으로 마우스 위치 얻기
        //Point mousePos = e.GetPosition(richTextBox);

        // 2. 단어 선택 (색 칠해주는 것까지 포함)
        //WordSearch.Interface.SelectRange(richTextBox, mousePos);

        // 3. 해석 요청 + MeaningBox에 표시
        await WordSearch.Interface.PrintMeaning(MeaningBox);
    }


    private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        // RichTextBox 내부 ScrollViewer 가져오기
        var scrollViewer = FindVisualChild<ScrollViewer>(richTextBox);
        if (scrollViewer != null)
        {
            if (e.Delta > 0)
                scrollViewer.LineUp();     // 휠 ↑
            else
                scrollViewer.LineDown();   // 휠 ↓
        }

        e.Handled = true; // 이벤트 버블링 방지 (필수)
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



}