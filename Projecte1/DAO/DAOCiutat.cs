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
    public class DAOCiutat : IDAOCiutat
    {
        private readonly ConnexioBd _connexio;

        public DAOCiutat(ConnexioBd connexio)
        {
            _connexio = connexio;
        }

        public ObservableCollection<Ciutat> GetCiutats()
        {
            ObservableCollection<Ciutat> ciutats = new ObservableCollection<Ciutat>();

            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"select * from ciutat";

                consulta.Transaction = _connexio.Database.CurrentTransaction?.GetDbTransaction();

                using (var reader = consulta.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("ID"));
                        string nom = reader.GetString(reader.GetOrdinal("nom"));

                        Ciutat c = new Ciutat(id, nom);
                        ciutats.Add(c);
                    }
                }
            }
            return ciutats;
        }

        public ObservableCollection<Ciutat> GetCiutatsByNom(string nom_filtre)
        {
            ObservableCollection<Ciutat> ciutats = new ObservableCollection<Ciutat>();

            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"select * from ciutat
                                where
                                ( @NOM_TEXT='%%' OR nom like @NOM_TEXT)";

                Utils.CrearParametre(consulta, "%" + nom_filtre + "%", "NOM_TEXT", System.Data.DbType.String);

                consulta.Transaction = _connexio.Database.CurrentTransaction?.GetDbTransaction();

                using (var reader = consulta.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("ID"));
                        string nom = reader.GetString(reader.GetOrdinal("nom"));

                        Ciutat c = new Ciutat(id, nom);
                        ciutats.Add(c);
                    }
                }
            }
            return ciutats;
        }
    }
}
