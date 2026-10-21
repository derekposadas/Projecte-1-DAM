using Model.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IDAO
{
    public interface IDAOCiutat
    {
        public ObservableCollection<Ciutat> GetCiutats();
        public ObservableCollection<Ciutat> GetCiutatsByNom(string nom);
    }
}
