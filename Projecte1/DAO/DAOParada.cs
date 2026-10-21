using IDAO;
using Model.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DAO
{
    public class DAOParada : IDAOParada
    {
        private readonly ConnexioBd _connexio;

        public DAOParada(ConnexioBd connexio)
        {
            _connexio = connexio;
        }

        public void editarParada(Parada parada)
        {
            throw new NotImplementedException();
        }

        public void eliminarParada(int posicio, int idTrajecte)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Parada> GetParades(int idTrajecte)
        {
            throw new NotImplementedException();
        }

        public void inserirParada(Parada parada)
        {
            throw new NotImplementedException();
        }
    }
}
