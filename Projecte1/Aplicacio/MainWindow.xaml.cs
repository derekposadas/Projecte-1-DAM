using Aplicacio.Pagines;
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
using Wpf.Ui.Controls;

namespace Aplicacio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string url, usr, pwd;
            url = propietatsOracle.Default.url;
            usr = propietatsOracle.Default.usr;
            pwd = propietatsOracle.Default.pwd;

            RootNavigation.Navigate(typeof(Consultors));
        }
    }
}