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
    public class DAOUsuari : IDAOUsuari
    {
        private readonly ConnexioBd _connexio;

        public DAOUsuari(ConnexioBd connexio)
        {
            _connexio = connexio;
        }

        public void bloquejaUsuari(Usuari user)
        {
            if (user.Tipus.Equals(TipusUsuari.BLOQUEJAT) || user.Tipus.Equals(TipusUsuari.ADMINISTRADOR)) return;

            user.Tipus = TipusUsuari.BLOQUEJAT;
        }

        public void desbloquejaUsuari(Usuari user)
        {
            if (user.Tipus.Equals(TipusUsuari.EDITOR) || user.Tipus.Equals(TipusUsuari.ADMINISTRADOR)) return;

            user.Tipus = TipusUsuari.EDITOR;
        }

        public ObservableCollection<Usuari> GetUsuaris()
        {
            return GetUsuarisByNom("");
        }

        public ObservableCollection<Usuari> GetUsuarisByNom(string nom_filtre, int offset = 0, int rows_per_page = -1)
        {
            ObservableCollection<Usuari> usuaris = new ObservableCollection<Usuari>();

            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"select * from usuari
                        where
                        ( :NOM_TEXT like nom )";

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
                        int id = reader.GetInt32(reader.GetOrdinal("ID"));
                        string? nom;
                        if (!reader.IsDBNull(reader.GetOrdinal("nom")))
                        {
                            nom = reader.GetString(reader.GetOrdinal("nom"));
                        }
                        else
                        {
                            nom = null;
                        }
                        string email = reader.GetString(reader.GetOrdinal("email"));
                        string password = reader.GetString(reader.GetOrdinal("password"));
                        char tipusc = reader.GetChar(reader.GetOrdinal("tipus"));
                        DateTime dataRegistre = reader.GetDateTime(reader.GetOrdinal("dataRegistre"));

                        TipusUsuari tipus = TipusUsuari.BLOQUEJAT;

                        switch (tipusc)
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

                        Usuari u = new Usuari(id, email, nom, password, tipus, dataRegistre);
                        usuaris.Add(u);
                    }
                }
            }
            return usuaris;
        }

        public bool insereixUsuari(Usuari user)
        {
            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            if (this.existeixUsuari(user))
            {
                return false;
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"insert into usuari (email, nom, password, tipus, dataRegistre)
                        values (:EMAIL_TEXT, :NOM_TEXT, :PASSWORD_TEXT, :TIPUS_TEXT, :DATAREGISTRE_TEXT)";

                Utils.CrearParametre(consulta, user.Email, "EMAIL_TEXT", System.Data.DbType.String);
                Utils.CrearParametre(consulta, user.Nom, "NOM_TEXT", System.Data.DbType.String);
                Utils.CrearParametre(consulta, user.Password, "PASSWORD_TEXT", System.Data.DbType.String);
                Utils.CrearParametre(consulta, 'E', "TIPUS_TEXT", System.Data.DbType.String);
                Utils.CrearParametre(consulta, user.DataRegistre, "DATAREGISTRE_TEXT", System.Data.DbType.DateTime);

                consulta.ExecuteNonQuery();
                return true;
            }
        }

        public bool existeixUsuari(Usuari user, int offset = 0, int rows_per_page = -1)
        {
            var connexio = _connexio.Database.GetDbConnection();

            if (connexio.State == System.Data.ConnectionState.Closed)
            {
                connexio.Open();
            }

            using (var consulta = connexio.CreateCommand())
            {
                consulta.CommandText =
                    @"select 1 from usuari
                        where
                        ( :EMAIL_TEXT = email )";

                if (rows_per_page > 0)
                {
                    consulta.CommandText += $" OFFSET {offset} ROWS FETCH NEXT {rows_per_page} ROWS ONLY";
                }

                Utils.CrearParametre(consulta, user.Email, "EMAIL_TEXT", System.Data.DbType.String);

                consulta.Transaction = _connexio.Database.CurrentTransaction?.GetDbTransaction();

                var reader = consulta.ExecuteReader();
                if (reader.Read())
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }
    }
}
