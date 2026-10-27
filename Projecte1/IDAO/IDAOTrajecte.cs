using Model.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IDAO
{
    public interface IDAOTrajecte
    {
        public ObservableCollection<Trajecte> GetTrajectes();
        public ObservableCollection<Trajecte> GetTrajectesByOrigen(Ciutat origen);
        public ObservableCollection<Trajecte> GetTrajectesByDesti(Ciutat desti);
        public ObservableCollection<Trajecte> GetTrajectes(Ciutat origen, Ciutat desti, string horaPas_filtre, int offset = 0, int rows_per_page = -1);
        public ObservableCollection<Trajecte> GetTrajectesByUsuari(Usuari user, int offset = 0, int rows_per_page = -1);
        public void validarTrajecte(Trajecte trajecte);
        public void invalidarTrajecte(Trajecte trajecte);
        public void inserirTrajecte(Trajecte trajecte);
        public void eliminarTrajecte(int id);
        public void editarTrajecte(Trajecte trajecte);
    }
}
