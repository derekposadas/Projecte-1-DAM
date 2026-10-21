using IDAO;
using Model.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DAO
{
    public class DAOUsuari : IDAOUsuari
    {
        private readonly ConnexioBd _connexio;

        public DAOUsuari(ConnexioBd connexio)
        {
            _connexio = connexio;
        }

        public void bloquejaUsuari(Usuari user)
        {
            throw new NotImplementedException();
        }

        public void desbloquejaUsuari(Usuari user)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Usuari> GetUsuaris()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Usuari> GetUsuarisByNom(string nom)
        {
            throw new NotImplementedException();
        }

        public void insereixUsuari(Usuari user)
        {
            throw new NotImplementedException();
        }

        public bool usuariActiu(string email, string password)
        {
            throw new NotImplementedException();
        }

        public bool validaContrasenya(string password)
        {
            throw new NotImplementedException();
        }
    }
}
