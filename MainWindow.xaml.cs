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
using CustomControl;
using UserAccountManager.Handlers;
//using WordSearch;

namespace AICA_Client;
/*
 * 단어 카드 보기 o
 * 단어 추가 예외처리 되는지 확인하기 o
 * 단어장 크기 맞추기 o
 * 자동로그인 o
 * aica 단어장 카드 보기 확인 o
 * 마이크 입력 테스트 페이지 열기 o
 * 발음비교 테스트
 *   발음비교 서버 연결 테스트
 *   마이크 입력 테스트
 *   비교 결과 받기 테스트
 * 이 문장에선 이렇게 쓰였어요??
 * 테스트
 *  단어장 테스트
 *      단어 삭제 - 단어 삭제 시 에이카 단어도 삭제되어야 함 o
 *      에이카 단어 삭제 - 이 기능은 없는 기능?
 *                        BE에 에이카 단어 삭제 기능이 없는 것으로 보여짐. 따라서 삭제함
 *      
 *  문장 테스트 o
 *      문장 업데이트
 *      문장 삭제
 *      
 * 에이카 단어장을 문장에 반영할 것 o
 * 
 * 문장 분석 직후 문장 리스트에 추가 안됨 o
 */
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private SharedViewModel vm;
    private LoadingPage loadingPage=null;
    public MainWindow()
    {
        InitializeComponent();
        
        this.Loaded += OnSearchBoxLoaded;
        this.Loaded += DoAutoLogin;
        this.ContentRendered += MainWindow_ContentRendered;
    }

    private async void MainWindow_ContentRendered(object? sender, EventArgs e)
    {
        Window_OpenLoadingPage(this, new RoutedEventArgs());

        //await Task.Yield();
        //await ThirdParty.Interface.Init();
        await Task.Run(() => ThirdParty.Interface.Init());
        Window_CloseLoadingPage(this, new RoutedEventArgs());

    }

    private void OnSearchBoxLoaded(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("loaded " + (this.DataContext is SharedViewModel));
        if (this.DataContext is SharedViewModel vm)
        {
            vm.IsLogin_LoginButtonHandler += Vm_PropertyChanged;
            Utility.UserSetting.Interface.Init(vm.SettingData);
        }
        
        //ThirdParty.Interface.Echo();
    }
    private async void DoAutoLogin(object sender, RoutedEventArgs e)
    {
        bool success;
        string msg;
        (success, msg) = await LoginHandler.AutoLogin();

        if (!success)
        {
            Debug.WriteLine(msg);
            return;
        }
        if(this.DataContext is SharedViewModel vm)
        {
            vm.IsLogin = true;
        }


        

        //loadingPage.Stop();
        //FrameContainer.Visibility = Visibility.Collapsed;
    }

    private void Vm_PropertyChanged(object? sender, EventArgs e)
    {
        if (this.DataContext is SharedViewModel vm)
        {
            Debug.WriteLine("Vm_PropertyChanged "+ vm.IsLogin);
            if (vm.IsLogin)
            {
                Login.Visibility = Visibility.Collapsed;
                RegistButton.Visibility = Visibility.Collapsed;
                MyPage.Visibility = Visibility.Visible;
            }
            else
            {
                Login.Visibility = Visibility.Visible;
                RegistButton.Visibility = Visibility.Visible;
                MyPage.Visibility = Visibility.Collapsed;
            }
        }
    }

    private void Login_Click(object sender, RoutedEventArgs e)
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
    public void OpenRegistFrame(object sender, RoutedEventArgs e)
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
    public void OpenRegistSuccessFrame()
    {
        FrameContainer.Visibility = Visibility.Visible;
        subFrame.Content = new RegistSuccess(this);
    }
    public void OpenPwdConfirmPageFrame()
    {
        FrameContainer.Visibility = Visibility.Visible;
        subFrame.Content = new PwdConfirmPage(this);
    }
    public void OpenLogoutSuccessPage()
    {
        FrameContainer.Visibility = Visibility.Visible;
        subFrame.Content = new LogoutSuccess(this);
    }
    public void OpenQuitMemberSuccessPage()
    {
        FrameContainer.Visibility = Visibility.Visible;
        subFrame.Content = new QuitMemberSuccess(this);
    }
    public void OpenPronunciationFrame(object sender, RoutedEventArgs e)
    {
        WordItem? item= e.OriginalSource as WordItem;
        Debug.WriteLine($"sender={sender?.GetType().Name}, src={e.Source?.GetType().Name}, osrc={e.OriginalSource?.GetType().Name}");

        Debug.WriteLine("item is null? " + (item != null));
        if (item != null) {
            Debug.WriteLine("in if");
            FrameContainer.Visibility = Visibility.Visible;
            subFrame.Content = new PronunciationTest(item.GetWord(),this);
        }
    }
    //private void RegistButton_Click(object sender, RoutedEventArgs e)
    //{
    //    this.OpenRegistFrame();
    //}

    private void MyPage_Click(object sender, RoutedEventArgs e)
    {
        OpenMyPageFrame();
    }

    private void Window_OpenLoadingPage(object sender, RoutedEventArgs e)
    {
        if(e is LoadingPageArgs)
        {
            string[]? message = (e as LoadingPageArgs)?.message;
            loadingPage = new LoadingPage(message);
        }
        else
        {
            loadingPage = new LoadingPage();
        }
            FrameContainer.Visibility = Visibility.Visible;
        subFrame.Content = loadingPage;
        loadingPage.Start();
    }

    private void Window_CloseLoadingPage(object sender, RoutedEventArgs e)
    {
        if (loadingPage==null)
        {
            return;
        }
        loadingPage.Stop();
        FrameContainer.Visibility = Visibility.Collapsed;
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