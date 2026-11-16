using Aplicacio.Finestres;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mostrarMenuPublic();
            RootNavigation.Navigate(typeof(Consultors));
        }

        // Menú per usuaris sense estar registrats (consultors)
        public void mostrarMenuPublic()
        {
            navConsultors.Visibility = Visibility.Visible;
            navLogin.Visibility = Visibility.Visible;

            navLogout.Visibility = Visibility.Collapsed;
            navCancel.Visibility = Visibility.Collapsed;
        }

        // Menú per usuaris registrats (editors i administradors)
        public void mostrarMenuPrivat()
        {
            navConsultors.Visibility = Visibility.Collapsed;
            navLogin.Visibility = Visibility.Collapsed;

            navLogout.Visibility = Visibility.Visible;
            navCancel.Visibility = Visibility.Collapsed;
        }

        // Menú per la pàgina de crear/modificar trajecte
        public void mostrarMenuTrajecte()
        {
            navConsultors.Visibility = Visibility.Collapsed;
            navLogin.Visibility = Visibility.Collapsed;

            navLogout.Visibility = Visibility.Visible;
            navCancel.Visibility = Visibility.Visible;
        }

        private void navCancel_Click(object sender, RoutedEventArgs e)
        {
            mostrarMenuPrivat();
            RootNavigation.Navigate(typeof(Editors));
        }

        private void navLogin_Click(object sender, RoutedEventArgs e)
        {
            IniciSessio login = new IniciSessio();
            login.Show();
        }

        private void navLogout_Click(object sender, RoutedEventArgs e)
        {
            mostrarMenuPublic();
            RootNavigation.Navigate(typeof(Consultors));
        }
    }
}