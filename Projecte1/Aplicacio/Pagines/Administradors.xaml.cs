using System;
using System.Collections.Generic;
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

namespace Aplicacio.Pagines
{
    /// <summary>
    /// Lógica de interacción para Administradors.xaml
    /// </summary>
    public partial class Administradors : Page
    {
        public Administradors()
        {
            InitializeComponent();
        }

        private void btnTrajectes_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            main.RootNavigation.Navigate(typeof(Administradors_Trajectes));
        }

        private void btnUsuaris_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            main.RootNavigation.Navigate(typeof(Administradors_Usuaris));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).mostrarMenuPrivat();
        }
    }
}
