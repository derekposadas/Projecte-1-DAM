using Model.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IDAO
{
    public interface IDAOEstacio
    {
        public ObservableCollection<Estacio> GetEstacions();
        public ObservableCollection<Estacio> GetEstacionsByNom(string nom_filtre, int offset = 0, int rows_per_page = -1);
        public void inserirEstacio(Estacio estacio);
        public bool existeixEstacio(Estacio estacio);
    }
}
