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
    }

    private void Canvas_MouseMove(object sender, MouseEventArgs e)
    {
        Point mousePos = e.GetPosition(richTextBox);
        WordSearch.Interface.SelectRange(richTextBox, mousePos);
    }
    private async void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        // 디버깅용 로그
        Debug.WriteLine("click");

        // 1. RichTextBox 기준으로 마우스 위치 얻기
        Point mousePos = e.GetPosition(richTextBox);

        // 2. 단어 선택 (색 칠해주는 것까지 포함)
        WordSearch.Interface.SelectRange(richTextBox, mousePos);

        // 3. 해석 요청 + MeaningBox에 표시
        await WordSearch.Interface.RequestDictionary(MeaningBox);
    }



}