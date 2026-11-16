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
                        int id = reader.GetInt32(reader.GetOrdinal("id"));
                        string nom = reader.GetString(reader.GetOrdinal("nom"));

                        Ciutat c = new Ciutat(id, nom);
                        ciutats.Add(c);
                    }
                }
            }
            return ciutats;
        }

        public Ciutat? GetCiutatByNom(string nom_filtre, int offset = 0, int rows_per_page = -1)
        {
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
                                ( upper(nom) like :NOM_TEXT)";

                if (rows_per_page > 0)
                {
                    consulta.CommandText += $" OFFSET {offset} ROWS FETCH NEXT {rows_per_page} ROWS ONLY";
                }

                Utils.CrearParametre(consulta, nom_filtre.ToUpper(), "NOM_TEXT", System.Data.DbType.String);

                consulta.Transaction = _connexio.Database.CurrentTransaction?.GetDbTransaction();

                using (var reader = consulta.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("id"));
                        string nom = reader.GetString(reader.GetOrdinal("nom"));

                        return new Ciutat(id, nom);
                    }
                }
            }
            return null;
        }

        public bool ExistsCiutat(string nom_filtre, int offset = 0, int rows_per_page = -1)
        {
            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"select 1 from ciutat
                                where
                                ( upper(nom) like :NOM_TEXT)";

                if (rows_per_page > 0)
                {
                    consulta.CommandText += $" OFFSET {offset} ROWS FETCH NEXT {rows_per_page} ROWS ONLY";
                }

                Utils.CrearParametre(consulta, nom_filtre.ToUpper(), "NOM_TEXT", System.Data.DbType.String);

                consulta.Transaction = _connexio.Database.CurrentTransaction?.GetDbTransaction();

                using (var reader = consulta.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
