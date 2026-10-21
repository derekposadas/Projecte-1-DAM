using Model.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IDAO
{
    public interface IDAOUsuari
    {
        public ObservableCollection<Usuari> GetUsuaris();
        public ObservableCollection<Usuari> GetUsuarisByNom(string nom);
        public bool usuariActiu(string email, string password);
        public void insereixUsuari(Usuari user);
        public void bloquejaUsuari(Usuari user);
        public void desbloquejaUsuari(Usuari user);
        public bool validaContrasenya(string password);
    }
}
