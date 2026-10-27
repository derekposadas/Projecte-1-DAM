using IDAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Model.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DAO
{
    public class DAOMitjaTransport : IDAOMitjaTransport
    {
        private readonly ConnexioBd _connexio;

        public DAOMitjaTransport(ConnexioBd connexio)
        {
            _connexio = connexio;
        }

        public ObservableCollection<MitjaTransport> GetMitjansTransport()
        {
            ObservableCollection<MitjaTransport> mitjansTransport = new ObservableCollection<MitjaTransport>();

            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"select * from mitja_transport";

                consulta.Transaction = _connexio.Database.CurrentTransaction?.GetDbTransaction();

                using (var reader = consulta.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("ID"));
                        string nom = reader.GetString(reader.GetOrdinal("nom"));

                        MitjaTransport mt = new MitjaTransport(id, nom);
                        mitjansTransport.Add(mt);
                    }
                }
            }
            return mitjansTransport;
        }
    }
}
