using CustomControl;
using CustomControl.ViewModel;
using System.ComponentModel;
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
using UserAccountManager.Handlers;
using Utility.Data.Sentence;
using Utility.RequestConst;
using Utility.TokenManager;

namespace AICA_Client;
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
        RequestConst.LoadConst();
        this.Loaded += OnSearchBoxLoaded;
        this.Loaded += DoAutoLogin;
        this.ContentRendered += MainWindow_ContentRendered;
    }

    private async void MainWindow_ContentRendered(object? sender, EventArgs e)
    {
        Window_OpenLoadingPage(this, new RoutedEventArgs());

        try
        {
            await Task.Run(() => ThirdParty.Interface.Init());
            Window_CloseLoadingPage(this, new RoutedEventArgs());
        }
        catch (Exception ex)
        {
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            MessageBox.Show($"자식프로세스를 찾지 못했습니다.\n({exeDir}child\\child.exe 이 없음)", "에러");
            Application.Current.Shutdown();
        }

    }

    private void OnSearchBoxLoaded(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine("loaded " + (this.DataContext is SharedViewModel));
        if (this.DataContext is SharedViewModel vm)
        {
            vm.IsLogin_LoginButtonHandler += Vm_PropertyChanged;
            Utility.UserSetting.Interface.Init(vm.SettingData);
        }
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

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        ThirdParty.Interface.Close();
    }
}