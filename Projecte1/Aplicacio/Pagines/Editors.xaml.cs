using DAO;
using IDAO;
using Model.model;
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
    /// Lógica de interacción para Editors.xaml
    /// </summary>
    public partial class Editors : Page
    {
        public Editors()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).mostrarMenuPrivat();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            main.RootNavigation.Navigate(typeof(EditarTrajecte));
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (lsvTrajectes.SelectedItem != null)
            {
                Trajecte t = (Trajecte)lsvTrajectes.SelectedItem;
                
            }
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            main.RootNavigation.Navigate(typeof(EditarTrajecte));
        }

        private void lsvTrajectes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvTrajectes.SelectedItem != null)
            {
                btnModificar.IsEnabled = true;
                btnEliminar.IsEnabled = true;
            } else
            {
                btnModificar.IsEnabled = false;
                btnEliminar.IsEnabled = false;
            }
        }
    }
}
