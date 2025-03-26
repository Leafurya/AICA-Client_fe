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
        WordSearch.Interface.InitStyle(richTextBox);
    }

    private void richTextBox_MouseMove(object sender, MouseEventArgs e)
    {
        Point mousePos = e.GetPosition(richTextBox);
        WordSearch.Interface.SelectRange(richTextBox, mousePos);
    }
}