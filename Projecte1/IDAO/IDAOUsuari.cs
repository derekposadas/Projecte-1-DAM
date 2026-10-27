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
        public ObservableCollection<Usuari> GetUsuarisByNom(string nom_filtre, int offset = 0, int rows_per_page = -1);
        public bool existeixUsuari(Usuari user);
        public bool insereixUsuari(Usuari user);
        public void bloquejaUsuari(Usuari user);
        public void desbloquejaUsuari(Usuari user);
    }
}
