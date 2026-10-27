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
    public class DAOEstacio : IDAOEstacio
    {
        private readonly ConnexioBd _connexio;

        public DAOEstacio(ConnexioBd connexio)
        {
            _connexio = connexio;
        }

        public bool existeixEstacio(Estacio estacio, int offset = 0, int rows_per_page = -1)
        {
            bool existeix = false;

            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"select 1 from estacio e
                        inner join ciutat c on c.id = e.IDCiutat
                                where
                                ( e.id = :ID_TEXT ) AND
                                ( e.nom = :NOM_TEXT ) AND
                                ( c.id = :ID_C_TEXT )";

                if (rows_per_page > 0)
                {
                    consulta.CommandText += $" OFFSET {offset} ROWS FETCH NEXT {rows_per_page} ROWS ONLY";
                }

                Utils.CrearParametre(consulta, estacio.Id, "ID_TEXT", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, estacio.Nom, "NOM_TEXT", System.Data.DbType.String);
                Utils.CrearParametre(consulta, estacio.Ciutat.Id, "ID_C_TEXT", System.Data.DbType.Int32);

                consulta.Transaction = _connexio.Database.CurrentTransaction?.GetDbTransaction();

                using (var reader = consulta.ExecuteReader())
                {
                    if (reader.Read()) existeix = true;
                }
                
                return existeix;
            }
        }

        public ObservableCollection<Estacio> GetEstacions()
        {
            return GetEstacionsByNom("");
        }

        public ObservableCollection<Estacio> GetEstacionsByNom(string nom_filtre, int offset = 0, int rows_per_page = -1)
        {
            ObservableCollection<Estacio> estacions = new ObservableCollection<Estacio>();

            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"select e.ID as idEstacio, e.nom as nomEstacio, e.latitud, e.longitud, c.id as idCiutat, c.nom as nomCiutat from estacio e
                        inner join ciutat c on e.IDCiutat = c.ID
                            where
                            ( nom = :NOM_TEXT )";

                if (rows_per_page > 0)
                {
                    consulta.CommandText += $" OFFSET {offset} ROWS FETCH NEXT {rows_per_page} ROWS ONLY";
                }

                Utils.CrearParametre(consulta, "%" + nom_filtre + "%", "NOM_TEXT", System.Data.DbType.String);

                consulta.Transaction = _connexio.Database.CurrentTransaction?.GetDbTransaction();

                using (var reader = consulta.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("idEstacio"));
                        string nom = reader.GetString(reader.GetOrdinal("nomEstacio"));
                        double latitud = reader.GetDouble(reader.GetOrdinal("latitud"));
                        double longitud = reader.GetDouble(reader.GetOrdinal("longitud"));
                        int idC = reader.GetInt32(reader.GetOrdinal("idCiutat"));
                        string nomC = reader.GetString(reader.GetOrdinal("nomCiutat"));

                        Ciutat c = new Ciutat(idC, nomC);
                        Estacio e = new Estacio(id, c, nom, latitud, longitud);
                        estacions.Add(e);
                    }
                }
            }
            return estacions;
        }

        public void inserirEstacio(Estacio estacio)
        {
            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            if (this.existeixEstacio(estacio))
            {
                return;
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"insert into estacio (ID, IDCiutat, nom, latitud, longitud)
                        values (:ID_TEXT, :IDC_TEXT, :NOM_TEXT, :LATITUD_TEXT, :LONGITUD_TEXT)";

                Utils.CrearParametre(consulta, estacio.Id, "ID_TEXT", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, estacio.Ciutat.Id, "IDC_TEXT", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, estacio.Nom, "NOM_TEXT", System.Data.DbType.String);
                Utils.CrearParametre(consulta, estacio.Latitud, "LATITUD_TEXT", System.Data.DbType.Double);
                Utils.CrearParametre(consulta, estacio.Longitud, "LONGITUD_TEXT", System.Data.DbType.Double);

                consulta.ExecuteNonQuery();
            }
        }
    }
}
