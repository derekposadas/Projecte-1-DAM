using IDAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
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

        public void editarParada(Parada parada, int nouOrdre)
        {
            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            // Recuperem totes les parades del trajecte
            ObservableCollection<Parada> parades =
                GetParades(parada.Trajecte.Id);

            // Comprovem que existeixi una parada a l'ordre de destinació
            Parada? paradaIntercanvi = parades.FirstOrDefault(
                p => p.Ordre == nouOrdre
            );

            if (paradaIntercanvi == null)
            {
                throw new InvalidOperationException(
                    "No es pot moure la parada perquè no hi ha cap parada a la posició de destinació."
                );
            }

            int ordreOriginal = parada.Ordre;
            int ordreTemporal = parades.Max(p => p.Ordre) + 1;

            using (var consulta = connexio.CreateCommand())
            {
                consulta.Transaction =
                    _connexio.Database.CurrentTransaction?.GetDbTransaction();

                // 1. Movem temporalment la parada seleccionada
                consulta.CommandText =
                    @"UPDATE parada
                      SET ordre = :ORDRE_TEMPORAL
                      WHERE ordre = :ORDRE_ORIGINAL
                        AND idTrajecte = :IDTRAJECTE_TEXT";

                Utils.CrearParametre(consulta, ordreTemporal, "ORDRE_TEMPORAL", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, ordreOriginal, "ORDRE_ORIGINAL", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, parada.Trajecte.Id, "IDTRAJECTE_TEXT", System.Data.DbType.Int32);

                consulta.ExecuteNonQuery();

                // 2. Posem la parada veïna a l'ordre original
                consulta.Parameters.Clear();

                consulta.CommandText =
                    @"UPDATE parada
                      SET ordre = :ORDRE_ORIGINAL
                      WHERE ordre = :ORDRE_DESTI
                        AND idTrajecte = :IDTRAJECTE_TEXT";

                Utils.CrearParametre(consulta, ordreOriginal, "ORDRE_ORIGINAL", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, nouOrdre, "ORDRE_DESTI", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, parada.Trajecte.Id, "IDTRAJECTE_TEXT", System.Data.DbType.Int32);

                consulta.ExecuteNonQuery();

                // 3. Posem la parada seleccionada al nou ordre
                consulta.Parameters.Clear();

                consulta.CommandText =
                    @"UPDATE parada
                      SET ordre = :ORDRE_DESTI,
                          nom = :NOM_TEXT,
                          horaPas = :HORAPAS_TEXT,
                          idEstacio = :ESTACIO_TEXT
                      WHERE ordre = :ORDRE_TEMPORAL
                        AND idTrajecte = :IDTRAJECTE_TEXT";

                Utils.CrearParametre(consulta, nouOrdre, "ORDRE_DESTI", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, parada.Nom, "NOM_TEXT", System.Data.DbType.String);
                Utils.CrearParametre(consulta, parada.HoraPas, "HORAPAS_TEXT", System.Data.DbType.String);
                Utils.CrearParametre(consulta, parada.Estacio.Id, "ESTACIO_TEXT", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, ordreTemporal, "ORDRE_TEMPORAL", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, parada.Trajecte.Id, "IDTRAJECTE_TEXT", System.Data.DbType.Int32);

                consulta.ExecuteNonQuery();
            }
        }

        public void eliminarParada(int ordre, int idTrajecte)
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
                        ( :ORDRE_TEXT = posicio ) and
                        ( :IDTRAJECTE_TEXT = idTrajecte )";

                Utils.CrearParametre(consulta, ordre, "ORDRE_TEXT", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, idTrajecte, "IDTRAJECTE_TEXT", System.Data.DbType.Int32);

                int q = consulta.ExecuteNonQuery();

                // Comprovem que s'ha eliminat una parada
                if (q > 0)
                {
                    consulta.Parameters.Clear();

                    // Fem que les parades amb ordre posterior redueixin en 1 l'ordre
                    consulta.CommandText =
                        @"UPDATE parada
                          SET ordre = ordre - 1
                          WHERE idTrajecte = :IDTRAJECTE_TEXT
                            AND ordre > :ORDRE_TEXT";

                    Utils.CrearParametre(consulta, idTrajecte, "IDTRAJECTE_TEXT", System.Data.DbType.Int32);
                    Utils.CrearParametre(consulta, ordre, "ORDRE_TEXT", System.Data.DbType.Int32);

                    consulta.ExecuteNonQuery();
                }
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

                        t.id AS idTrajecte,
                        t.nom AS nomTrajecte,
                        t.dataCreacio,
                        t.validat,

                        mt.id AS idMitjaTransport,
                        mt.nom AS nomMitjaTransport,

                        u.id AS idUsuari,
                        u.email AS emailUsuari,
                        u.nom AS nomUsuari,
                        u.password AS passwordUsuari,
                        u.tipus AS tipusUsuari,
                        u.dataRegistre,

                        e.id AS idEstacio,
                        e.nom AS nomEstacio,
                        e.latitud,
                        e.longitud,

                        c.id AS idCiutat,
                        c.nom AS nomCiutat

                    FROM parada p

                    JOIN trajecte t
                        ON t.id = p.idTrajecte

                    JOIN mitja_transport mt
                        ON mt.id = t.mitjaTransport

                    JOIN usuari u
                        ON u.id = t.usuariCreador

                    JOIN estacio e
                        ON e.id = p.idEstacio

                    JOIN ciutat c
                        ON c.id = e.IDCiutat

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
                    @"insert into parada (idTrajecte, nom, horaPas, idEstacio)
                        values (:IDT_TEXT, :NOM_TEXT, :HORAPAS_TEXT, :IDE_TEXT)";

                Utils.CrearParametre(consulta, parada.Trajecte.Id, "IDT_TEXT", System.Data.DbType.Int32);
                Utils.CrearParametre(consulta, parada.Nom, "NOM_TEXT", System.Data.DbType.String);
                Utils.CrearParametre(consulta, parada.HoraPas, "HORAPAS_TEXT", System.Data.DbType.String);
                Utils.CrearParametre(consulta, parada.Estacio.Id, "IDE_TEXT", System.Data.DbType.Int32);

                consulta.ExecuteNonQuery();
            }
        }
    }
}
