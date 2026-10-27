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
    public class DAOParada : IDAOParada
    {
        private readonly ConnexioBd _connexio;

        public DAOParada(ConnexioBd connexio)
        {
            _connexio = connexio;
        }

        public void editarParada(Parada parada)
        {
            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"select nom from mitja_transport";

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

        public void eliminarParada(int posicio, int idTrajecte)
        {
            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"delete from parada
                        where
                        ( :POSICIO_TEXT = posicio ) and
                        ( :IDTRAJECTE_TEXT = IDTrajecte )";

                Utils.CrearParametre(consulta, posicio, "POSICIO_TEXT", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, idTrajecte, "IDTRAJECTE_TEXT", System.Data.DbType.Int32);

                consulta.ExecuteNonQuery();
                // Fa falta comprovar que no hi hagin parades del mateix trajecte posteriors a la que volem eliminar
                // En cas que si hi hagin, s'hauria de modificar les parades posteriors i posar-li l'ordre--
            }
        }

        public ObservableCollection<Parada> GetParades(int idTrajecte, int offset = 0, int rows_per_page = -1)
        {
            ObservableCollection<Parada> parades = new ObservableCollection<Parada>();

            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"SELECT
                        p.ordre,
                        p.nom,
                        p.horaPas,

                        t.ID AS idTrajecte,
                        t.nom AS nomTrajecte,
                        t.dataCreacio,
                        t.validat,

                        mt.ID AS idMitjaTransport,
                        mt.nom AS nomMitjaTransport,

                        u.ID AS idUsuari,
                        u.email AS emailUsuari,
                        u.nom AS nomUsuari,
                        u.password AS passwordUsuari,
                        u.tipus AS tipusUsuari,
                        u.dataRegistre,

                        e.ID AS idEstacio,
                        e.nom AS nomEstacio,
                        e.latitud,
                        e.longitud,

                        c.ID AS idCiutat,
                        c.nom AS nomCiutat

                    FROM parada p

                    JOIN trajecte t
                        ON t.ID = p.idTrajecte

                    JOIN mitja_transport mt
                        ON mt.ID = t.mitjaTransport

                    JOIN usuari u
                        ON u.ID = t.usuariCreador

                    JOIN estacio e
                        ON e.ID = p.idEstacio

                    JOIN ciutat c
                        ON c.ID = e.IDCiutat

                    WHERE p.idTrajecte = :ID_TRAJECTE";

                if (rows_per_page > 0)
                {
                    consulta.CommandText += $" OFFSET {offset} ROWS FETCH NEXT {rows_per_page} ROWS ONLY";
                }

                Utils.CrearParametre(consulta, idTrajecte, "ID_TRAJECTE", System.Data.DbType.Int32);

                consulta.Transaction = _connexio.Database.CurrentTransaction?.GetDbTransaction();

                using (var reader = consulta.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Dades de la parada
                        int ordre = reader.GetInt32(reader.GetOrdinal("ordre"));

                        string nom = reader.GetString(reader.GetOrdinal("nom"));

                        string horaPas = reader.GetString(reader.GetOrdinal("horaPas"));

                        // Dades del mitjà de transport
                        int idMt = reader.GetInt32(reader.GetOrdinal("idMitjaTransport"));

                        string nomMt = reader.GetString(reader.GetOrdinal("nomMitjaTransport"));

                        MitjaTransport mt = new MitjaTransport(idMt, nomMt);

                        // Dades de l'usuari
                        int idU = reader.GetInt32(reader.GetOrdinal("idUsuari"));

                        string email = reader.GetString(reader.GetOrdinal("emailUsuari"));

                        string? nomU = reader.IsDBNull(reader.GetOrdinal("nomUsuari")) ? null : reader.GetString(reader.GetOrdinal("nomUsuari"));

                        string pwd = reader.GetString(reader.GetOrdinal("passwordUsuari"));

                        string tipusChar = reader.GetString(reader.GetOrdinal("tipusUsuari"));

                        DateTime dataRegistre = reader.GetDateTime(reader.GetOrdinal("dataRegistre"));

                        TipusUsuari tipus = TipusUsuari.BLOQUEJAT;

                        switch (tipusChar[0])
                        {
                            case 'A':
                                tipus = TipusUsuari.ADMINISTRADOR;
                                break;

                            case 'E':
                                tipus = TipusUsuari.EDITOR;
                                break;

                            case 'B':
                                tipus = TipusUsuari.BLOQUEJAT;
                                break;
                        }

                        Usuari u = new Usuari(idU, email, nomU, pwd, tipus, dataRegistre);

                        // Dades del trajecte
                        int idT = reader.GetInt32(reader.GetOrdinal("idTrajecte"));

                        string nomT = reader.GetString(reader.GetOrdinal("nomTrajecte"));

                        DateTime dataCreacio = reader.GetDateTime(reader.GetOrdinal("dataCreacio"));

                        bool validat = reader.GetInt32(reader.GetOrdinal("validat")) == 1;

                        Trajecte trajecte = new Trajecte(idT, nomT, dataCreacio, validat, mt, u);

                        // Dades de la ciutat
                        int idC = reader.GetInt32(reader.GetOrdinal("idCiutat"));

                        string nomC = reader.GetString(reader.GetOrdinal("nomCiutat"));

                        Ciutat ciutat = new Ciutat(idC, nomC);

                        // Dades de l'estació
                        int idE = reader.GetInt32(reader.GetOrdinal("idEstacio"));

                        string nomE = reader.GetString(reader.GetOrdinal("nomEstacio"));

                        double latitud = reader.GetDouble(reader.GetOrdinal("latitud"));

                        double longitud = reader.GetDouble(reader.GetOrdinal("longitud"));

                        Estacio estacio = new Estacio(idE, ciutat, nomE, latitud, longitud);

                        // Crear la parada
                        Parada parada = new Parada(ordre, trajecte, nom, horaPas, estacio);

                        parades.Add(parada);
                    }
                }
            }

            return parades;
        }

        public void inserirParada(Parada parada)
        {
            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"insert into parada (ordre, idTrajecte, nom, horaPas, idEstacio)
                        values (:ORDRE_TEXT, :IDT_TEXT, :NOM_TEXT, :HORAPAS_TEXT, :IDE_TEXT)";

                Utils.CrearParametre(consulta, parada.Ordre, "ORDRE_TEXT", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, parada.Trajecte.Id, "IDT_TEXT", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, parada.Nom, "NOM_TEXT", System.Data.DbType.String);
                Utils.CrearParametre(consulta, parada.HoraPas, "HORAPAS_TEXT", System.Data.DbType.String);
                Utils.CrearParametre(consulta, parada.Estacio.Id, "IDE_TEXT", System.Data.DbType.Int32);

                consulta.ExecuteNonQuery();
            }
        }
    }
}
