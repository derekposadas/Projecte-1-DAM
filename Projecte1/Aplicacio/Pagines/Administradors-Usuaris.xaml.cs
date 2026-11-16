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
    /// Lógica de interacción para Administradors_Usuaris.xaml
    /// </summary>
    public partial class Administradors_Usuaris : Page
    {
        ObservableCollection<Usuari> usuaris = new ObservableCollection<Usuari>();

        public Administradors_Usuaris()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
