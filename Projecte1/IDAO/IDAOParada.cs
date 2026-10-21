using Model.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IDAO
{
    public interface IDAOParada
    {
        public ObservableCollection<Parada> GetParades(int idTrajecte);
        public void inserirParada(Parada parada);
        public void eliminarParada(int posicio, int idTrajecte);
        public void editarParada(Parada parada);
    }
}
