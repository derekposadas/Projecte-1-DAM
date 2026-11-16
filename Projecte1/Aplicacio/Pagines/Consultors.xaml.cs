using DAO;
using IDAO;
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
    /// Lógica de interacción para Consultors.xaml
    /// </summary>
    public partial class Consultors : Page
    {
        private ObservableCollection<Trajecte> trajectes = new ObservableCollection<Trajecte>();
        private ObservableCollection<Ciutat> ciutats = new ObservableCollection<Ciutat>();

        private Ciutat? origenSeleccionat, destiSeleccionat;

        private string connexio;

        public Consultors()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            connexio = propietatsOracle.Default.ConnexióOracle;

            obtenirCiutats();
        }

        private void actualitzarTrajectes(Ciutat? origen, Ciutat? desti, string horaPas)
        {
            OracleFactory.getUOW(connexio, uow =>
            {
                IDAOTrajecte dao_trajecte = uow.DAOTrajectes;
                trajectes = dao_trajecte.GetTrajectes(origen, desti, horaPas);
                lsvTrajectes.ItemsSource = trajectes;
            });
        }

        private void obtenirCiutats()
        {
            OracleFactory.getUOW(connexio, uow =>
            {
                IDAOCiutat dao_ciutat = uow.DAOCiutats;
                ciutats = dao_ciutat.GetCiutats();
            });
        }

        private void txb_TextChanged(object sender, TextChangedEventArgs e)
        {
            OracleFactory.getUOW(connexio, uow =>
            {
                IDAOCiutat dao_ciutat = uow.DAOCiutats;
                if (dao_ciutat.ExistsCiutat(txbOrigen.Text))
                {
                    origenSeleccionat = dao_ciutat.GetCiutatByNom(txbOrigen.Text);
                }
                if (dao_ciutat.ExistsCiutat(txbDesti.Text))
                {
                    destiSeleccionat = dao_ciutat.GetCiutatByNom(txbDesti.Text);
                    
                }
            });
            actualitzarTrajectes(origenSeleccionat, destiSeleccionat, txbHoraSortida.Text);
        }

        private void lstOrigen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstOrigen.SelectedItem is Ciutat ciutat)
            {
                txbOrigen.Text = ciutat.Nom;

                origenSeleccionat = ciutat;

                txbOrigen.CaretIndex = txbOrigen.Text.Length;

                popupOrigen.IsOpen = false;
            }
        }

        private void btnFiltre_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
