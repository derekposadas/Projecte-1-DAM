using Model.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Lógica de interacción para EditarTrajecte.xaml
    /// </summary>
    public partial class EditarTrajecte : Page
    {
        List<MitjaTransport> mitjaTransports = new List<MitjaTransport>();
        ObservableCollection<Estacio> estacions = new ObservableCollection<Estacio>();

        public EditarTrajecte()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).mostrarMenuTrajecte();
        }

        private void btnAfegirTrajecte_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAfegirParada_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEliminarParada_Click(object sender, RoutedEventArgs e)
        {
            Parada? p = lsvParades.SelectedItem as Parada;
            if (p != null)
            {

            }
        }

        private void btnModificarParada_Click(object sender, RoutedEventArgs e)
        {
            Parada? p = lsvParades.SelectedItem as Parada;
            if (p != null)
            {
                txbNomParada.Text = p.Nom;
                txbHoraPas.Text = p.HoraPas;
                txbEstacioParada.Text = p.Estacio.Nom;
            }
        }

        private void lsvParades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvParades.SelectedItem != null)
            {
                btnEliminarParada.IsEnabled = true;
                btnModificarParada.IsEnabled = true;
            } else
            {
                btnEliminarParada.IsEnabled = false;
                btnModificarParada.IsEnabled = false;
            }
        }
    }
}
