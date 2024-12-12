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

namespace G24W1502RestAPI;


public partial class MainWindow : Window
{
    private GameViewModel vm = new GameViewModel();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = vm;

        //GameGrid.ItemsSource = vm.Games;

        InitializeGames();
    }

    private async void InitializeGames()
    {
        await vm.GetGames();


    }

    public void StartWebBrowser(object sender, EventArgs e)
    {
        string url = (string)((Button)sender).Tag;
        Process.Start(
            new ProcessStartInfo("cmd", $"/c start {url}")
            {
                CreateNoWindow = true
            }
        );
    }
}