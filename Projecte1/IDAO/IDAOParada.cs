using Model.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IDAO
{
    public interface IDAOParada
    {
        public ObservableCollection<Parada> GetParades(int idTrajecte_filtre, int offset = 0, int rows_per_page = -1);
        public void inserirParada(Parada parada);
        public void eliminarParada(int posicio_filtre, int idTrajecte_filtre);
        public void editarParada(Parada parada);
    }
}
