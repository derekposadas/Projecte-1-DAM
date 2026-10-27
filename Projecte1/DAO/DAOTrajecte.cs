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
    public class DAOTrajecte : IDAOTrajecte
    {
        private readonly ConnexioBd _connexio;

        public DAOTrajecte(ConnexioBd connexio)
        {
            _connexio = connexio;
        }

        public void editarTrajecte(Trajecte trajecte)
        {
            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"update trajecte set 
                        nom = :NOM_TEXT,
                        validat = :VALIDAT_TEXT,
                        mitjaTransport = :MITJATRANSPORT_TEXT
                            where ID = :ID_TEXT";

                Utils.CrearParametre(consulta, trajecte.Nom, "NOM_TEXT", System.Data.DbType.String);
                if (trajecte.Validat == true)
                    Utils.CrearParametre(consulta, 1, "VALIDAT_TEXT", System.Data.DbType.Int32);
                else
                    Utils.CrearParametre(consulta, 0, "VALIDAT_TEXT", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, trajecte.MitjaTransport.Id, "MITJATRANSPORT_NOM", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, trajecte.Id, "ID_TEXT", System.Data.DbType.Int32);

                consulta.ExecuteNonQuery();
            }
        }

        public void eliminarTrajecte(int id)
        {
            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"delete from trajecte where id = :ID_TEXT";

                Utils.CrearParametre(consulta, id, "ID_TEXT", System.Data.DbType.Int32);

                consulta.ExecuteNonQuery();
            }
        }

        public ObservableCollection<Trajecte> GetTrajectes()
        {
            return GetTrajectes(null, null, "");
        }

        public ObservableCollection<Trajecte> GetTrajectes(Ciutat? origen, Ciutat? desti, string horaPas, int offset = 0, int rows_per_page = -1)
        {
            ObservableCollection<Trajecte> trajectes = new ObservableCollection<Trajecte>();

            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"SELECT DISTINCT t.*,
                        mt.nom AS nomMitjaTransport,
                        u.nom AS nomUsuari,
                        u.email AS emailUsuari,
                        u.password AS passwordUsuari,
                        u.tipus AS tipusUsuari,
                        u.dataRegistre AS dataRegistreUsuari
                        FROM trajecte t
                        JOIN mitja_transport mt
                            ON mt.id = t.mitjaTransport
                        JOIN usuari u
                            ON u.id = t.usuariCreador
                        JOIN parada pOrigen
                            ON pOrigen.idTrajecte = t.id
                        JOIN estacio eOrigen
                            ON eOrigen.id = pOrigen.idEstacio
                        JOIN ciutat cOrigen
                            ON cOrigen.id = eOrigen.idCiutat
                        JOIN parada pDesti
                            ON pDesti.idTrajecte = t.id
                        JOIN estacio eDesti
                            ON eDesti.id = pDesti.idEstacio
                        JOIN ciutat cDesti
                            ON cDesti.id = eDesti.idCiutat
                        WHERE (cOrigen.nom LIKE :ORIGEN_TEXT OR :ORIGEN_TEXT = '%')
                        AND (cDesti.nom LIKE :DESTI_TEXT OR :DESTI_TEXT = '%')
                        AND pOrigen.horaPas = :HORAPAS_TEXT
                        AND pOrigen.ordre < pDesti.ordre";

                if (rows_per_page > 0)
                {
                    consulta.CommandText += $" OFFSET {offset} ROWS FETCH NEXT {rows_per_page} ROWS ONLY";
                }

                string ciutatOrigen = origen == null ? "%"  : "%" + origen.Nom + "%";
                string ciutatDesti = desti == null ? "%" : "%" + desti.Nom + "%";

                Utils.CrearParametre(consulta, ciutatOrigen, "ORIGEN_TEXT", System.Data.DbType.String);
                Utils.CrearParametre(consulta, ciutatDesti, "DESTI_TEXT", System.Data.DbType.String);
                Utils.CrearParametre( consulta, horaPas, "HORAPAS_TEXT", System.Data.DbType.String);

                consulta.Transaction = _connexio.Database.CurrentTransaction?.GetDbTransaction();

                using (var reader = consulta.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("ID"));

                        string nom = reader.GetString(reader.GetOrdinal("nom"));

                        DateTime dataCreacio = reader.GetDateTime(reader.GetOrdinal("dataCreacio"));

                        bool validat = reader.GetInt32(reader.GetOrdinal("validat")) == 1;

                        // Mitjà de transport
                        int idMt = reader.GetInt32(reader.GetOrdinal("mitjaTransport"));

                        string nomMt = reader.GetString(reader.GetOrdinal("nomMitjaTransport"));

                        MitjaTransport mt = new MitjaTransport(idMt, nomMt);

                        // Usuari creador
                        int idU = reader.GetInt32(reader.GetOrdinal("usuariCreador"));

                        string? nomU = reader.IsDBNull(reader.GetOrdinal("nomUsuari")) ? null : reader.GetString(reader.GetOrdinal("nomUsuari"));

                        string email = reader.GetString(reader.GetOrdinal("emailUsuari"));

                        string pwd = reader.GetString(reader.GetOrdinal("passwordUsuari"));

                        string tipusc = reader.GetString(reader.GetOrdinal("tipusUsuari"));

                        DateTime dataRegistre = reader.GetDateTime(reader.GetOrdinal("dataRegistreUsuari"));

                        TipusUsuari tipus = TipusUsuari.BLOQUEJAT;

                        switch (tipusc[0])
                        {
                            case 'E':
                                tipus = TipusUsuari.EDITOR;
                                break;
                            case 'A':
                                tipus = TipusUsuari.ADMINISTRADOR;
                                break;
                            case 'B':
                                tipus = TipusUsuari.BLOQUEJAT;
                                break;
                        }

                        Usuari u = new Usuari(idU, email, nomU, pwd, tipus, dataRegistre);

                        Trajecte t = new Trajecte(id, nom, dataCreacio, validat, mt, u);

                        trajectes.Add(t);
                    }
                }
            }

            return trajectes;
        }

        public ObservableCollection<Trajecte> GetTrajectesByDesti(Ciutat desti)
        {
            return GetTrajectes(null, desti, "");
        }

        public ObservableCollection<Trajecte> GetTrajectesByOrigen(Ciutat origen)
        {
            return GetTrajectes(origen, null, "");
        }

        public ObservableCollection<Trajecte> GetTrajectesByUsuari(Usuari user, int offset = 0, int rows_per_page = -1)
        {
            ObservableCollection<Trajecte> trajectes = new ObservableCollection<Trajecte>();

            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"select t.*, m.nom as nomM from trajecte t inner join mitja_transport m on t.mitjaTransport = m.id
                        where 
                        ( usuariCreador = :USUARI_TEXT )";

                if (rows_per_page > 0)
                {
                    consulta.CommandText += $" OFFSET {offset} ROWS FETCH NEXT {rows_per_page} ROWS ONLY";
                }

                Utils.CrearParametre(consulta, user.Id, "USUARI_TEXT", System.Data.DbType.Int32);

                consulta.Transaction = _connexio.Database.CurrentTransaction?.GetDbTransaction();

                using (var reader = consulta.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("ID"));
                        string nom = reader.GetString(reader.GetOrdinal("nom"));
                        DateTime dataCreacio = reader.GetDateTime(reader.GetOrdinal("dataCreacio"));
                        int ivalidat = reader.GetInt32(reader.GetOrdinal("validat"));
                        bool validat = reader.GetInt32(reader.GetOrdinal("validat")) == 1;

                        int idMitjaTransport = reader.GetInt32(reader.GetOrdinal("mitjaTransport"));
                        int idUsuari = reader.GetInt32(reader.GetOrdinal("usuariCreador"));

                        string nomMitjaTransport = reader.GetString(reader.GetOrdinal("nomM"));

                        MitjaTransport m = new MitjaTransport(idMitjaTransport, nomMitjaTransport);
                        Trajecte t = new Trajecte(id, nom, dataCreacio, validat, m, user);
                        trajectes.Add(t);
                    }
                }
            }
            return trajectes;
        }

        public void inserirTrajecte(Trajecte trajecte)
        {
            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"insert into trajecte (ID, nom, dataCreacio, validat, mitjaTransport, usuariCreador)
                        values (:ID_TEXT, :NOM_TEXT, :DATACREACIO_TEXT, :VALIDAT_TEXT, :MITJATRANSPORT_TEXT, :USUARI_TEXT)";

                Utils.CrearParametre(consulta, trajecte.Id, "ID_TEXT", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, trajecte.Nom, "NOM_TEXT", System.Data.DbType.String);
                Utils.CrearParametre(consulta, trajecte.DataCreacio, "NOM_TEXT", System.Data.DbType.DateTime);
                if (trajecte.Validat == true) 
                    Utils.CrearParametre(consulta, 1, "VALIDAT_TEXT", System.Data.DbType.Int32);
                else
                    Utils.CrearParametre(consulta, 0, "VALIDAT_TEXT", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, trajecte.MitjaTransport.Id, "MITJATRANSPORT_TEXT", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, trajecte.UsuariCreador.Id, "USUARI_TEXT", System.Data.DbType.Int32);

                consulta.ExecuteNonQuery();
            }

        }

        public void invalidarTrajecte(Trajecte trajecte)
        {
            trajecte.Validat = false;
            editarTrajecte(trajecte);
        }

        public void validarTrajecte(Trajecte trajecte)
        {
            trajecte.Validat = true;
            editarTrajecte(trajecte);
        }
    }
}
