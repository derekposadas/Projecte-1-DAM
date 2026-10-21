using IDAO;
using Microsoft.EntityFrameworkCore;
using Model.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DAO
{
    public class DAOTrajecte : IDAOTrajecte
    {
        private readonly ConnexioBd _connexio;

        public DAOTrajecte(ConnexioBd connexio)
        {
            _connexio = connexio;
        }

        public void editarTrajecte(Trajecte trajecte)
        {
            throw new NotImplementedException();
        }

        public void eliminarTrajecte(int id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Trajecte> GetTrajectes()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Trajecte> GetTrajectes(Ciutat origen, Ciutat desti, string horaPas)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Trajecte> GetTrajectesByDesti(Ciutat desti)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Trajecte> GetTrajectesByOrigen(Ciutat origen)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Trajecte> GetTrajectesByUsuari(Usuari user)
        {
            throw new NotImplementedException();
        }

        public void inserirTrajecte(Trajecte trajecte)
        {
            throw new NotImplementedException();
        }

        public void invalidarTrajecte(Trajecte trajecte)
        {
            throw new NotImplementedException();
        }

        public void validarTrajecte(Trajecte trajecte)
        {
            throw new NotImplementedException();
        }
    }
}
