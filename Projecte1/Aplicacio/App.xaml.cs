using System.Configuration;
using System.Data;
using System.Windows;

namespace Aplicacio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var main = new MainWindow();
            MainWindow = main;
            main.Show();
        }
    }

}
