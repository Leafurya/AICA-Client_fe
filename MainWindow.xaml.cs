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
    private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        Debug.WriteLine("click");
        WordSearch.Interface.RequestDictionary();
    }
}