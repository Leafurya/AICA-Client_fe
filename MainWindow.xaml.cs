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
using Utility.Data.Sentence;
using CustomControl.ViewModel;
using System.ComponentModel;
using Utility.TokenManager;
//using WordSearch;

namespace AICA_Client;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private SharedViewModel vm;
    public MainWindow()
    {
        InitializeComponent();
        this.Loaded += OnSearchBoxLoaded;
        
    }
    private void OnSearchBoxLoaded(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("loaded "+ (this.DataContext is SharedViewModel));
        if (this.DataContext is SharedViewModel vm)
        {
            vm.IsLogin_LoginButtonHandler += Vm_PropertyChanged;
            Utility.UserSetting.Interface.Init(vm.SettingData);
        }
        ThirdParty.Interface.Init();
        ThirdParty.Interface.Echo();
    }

    private void Vm_PropertyChanged(object? sender, EventArgs e)
    {
        if (this.DataContext is SharedViewModel vm)
        {
            Debug.WriteLine("Vm_PropertyChanged "+ vm.IsLogin);
            if (vm.IsLogin)
            {
                Ligin.Visibility = Visibility.Collapsed;
                MyPage.Visibility = Visibility.Visible;
            }
            else
            {
                Ligin.Visibility = Visibility.Visible; 
                MyPage.Visibility = Visibility.Collapsed;
            }
        }
    }

    private void Ligin_Click(object sender, RoutedEventArgs e)
    {
        FrameContainer.Visibility = Visibility.Visible;
        subFrame.Content = new Login(this);
    }
    public void CloseFrameContainer()
    {
        FrameContainer.Visibility = Visibility.Collapsed;
    }

    //private void Regist_Click(object sender, RoutedEventArgs e)
    //{
    //    FrameContainer.Visibility = Visibility.Visible;
    //    subFrame.Content = new Regist(this);
    //}
    public void OpenRegistFrame()
    {
        FrameContainer.Visibility = Visibility.Visible;
        subFrame.Content = new Regist(this);
    }
    public void OpenLoginFrame()
    {
        FrameContainer.Visibility = Visibility.Visible;
        subFrame.Content = new Login(this);
    }
    public void OpenEditInfoFrame()
    {
        FrameContainer.Visibility = Visibility.Visible;
        subFrame.Content = new EditUserInfo(this);
    }
    public void OpenMyPageFrame()
    {
        FrameContainer.Visibility = Visibility.Visible;
        subFrame.Content = new MyPage(this);
    }

    private void MyPage_Click(object sender, RoutedEventArgs e)
    {
        OpenMyPageFrame();
    }

    //private async void refresh_Click(object sender, RoutedEventArgs e)
    //{
    //    (bool suc,string msg)=await TokenManager.RefreshTokenAsync();
    //    MessageBox.Show(msg, "알림");

    //}

    //private void Canvas_MouseMove(object sender, MouseEventArgs e)
    //{
    //    Point mousePos = e.GetPosition(richTextBox);
    //    WordSearch.Interface.SelectRange(richTextBox, mousePos);
    //}
    //private async void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    //{
    //    WordSearch.Interface.HighlightPOS(richTextBox);

    //    //Debug.WriteLine("click");

    //    // 1. RichTextBox 기준으로 마우스 위치 얻기
    //    //Point mousePos = e.GetPosition(richTextBox);

    //    // 2. 단어 선택 (색 칠해주는 것까지 포함)
    //    //WordSearch.Interface.SelectRange(richTextBox, mousePos);

    //    // 3. 해석 요청 + MeaningBox에 표시
    //    await WordSearch.Interface.PrintMeaning(MeaningBox);
    //}


    //private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
    //{
    //    // RichTextBox 내부 ScrollViewer 가져오기
    //    var scrollViewer = FindVisualChild<ScrollViewer>(richTextBox);
    //    if (scrollViewer != null)
    //    {
    //        if (e.Delta > 0)
    //            scrollViewer.LineUp();     // 휠 ↑
    //        else
    //            scrollViewer.LineDown();   // 휠 ↓
    //    }

    //    e.Handled = true; // 이벤트 버블링 방지 (필수)
    //}
    //public static T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
    //{
    //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
    //    {
    //        var child = VisualTreeHelper.GetChild(parent, i);
    //        if (child is T correctlyTyped)
    //            return correctlyTyped;

    //        var result = FindVisualChild<T>(child);
    //        if (result != null)
    //            return result;
    //    }
    //    return null;
    //}



}