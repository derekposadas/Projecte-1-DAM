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

        public bool existeixEstacio(Estacio estacio)
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
                        inner join ciutat c on c.id = e.idCiutat
                                where
                                ( @ID_TEXT=-1 OR e.id = @ID_TEXT ) AND
                                ( @NOM_TEXT='%%' OR e.nom = @NOM_TEXT ) AND
                                ( @ID_C_TEXT=-1 OR c.id = @ID_C_TEXT )";

                Utils.CrearParametre(consulta, estacio.Id, "ID_TEXT", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, estacio.Nom, "NOM_TEXT", System.Data.DbType.String);
                Utils.CrearParametre(consulta, estacio.Ciutat.Id, "ID_C_TEXT", System.Data.DbType.Int32);

                consulta.Transaction = _connexio.Database.CurrentTransaction?.GetDbTransaction();

                var reader = consulta.ExecuteReader();
                if (reader.Read()) existeix = true;

                return existeix;
            }
        }

        public ObservableCollection<Estacio> GetEstacions()
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
                        inner join ciutat c on e.IDCiutat = c.ID";

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

        public ObservableCollection<Estacio> GetEstacionsByNom(string nom_filtre)
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
                            ( @NOM_TEXT='%%' OR nom = @NOM_TEXT )";

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
                    @"insert into estacio (";

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
        }
    }
}
