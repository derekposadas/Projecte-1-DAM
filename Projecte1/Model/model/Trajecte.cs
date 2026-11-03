using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model.model
{
    public class Trajecte : INotifyPropertyChanged
    {
        private int id;
        private string nom;
        private DateTime dataCreacio;
        private bool validat;
        private MitjaTransport mitjaTransport;
        private Usuari usuariCreador;

        public event PropertyChangedEventHandler? PropertyChanged;

        // Constructor per trajectes nous
        public Trajecte(string nom, DateTime dataCreacio, bool validat, MitjaTransport mitjaTransport, Usuari usuariCreador)
        {
            Nom = nom;
            DataCreacio = dataCreacio;
            Validat = validat;
            MitjaTransport = mitjaTransport;
            UsuariCreador = usuariCreador;
        }

        // Constructor per trajectes de la BD
        public Trajecte(int id, string nom, DateTime dataCreacio, bool validat, MitjaTransport mitjaTransport, Usuari usuariCreador)
        {
            Id = id;
            Nom = nom;
            DataCreacio = dataCreacio;
            Validat = validat;
            MitjaTransport = mitjaTransport;
            UsuariCreador = usuariCreador;
        }

        #region propietats
        public int Id { get => id; private set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public DateTime DataCreacio { get => dataCreacio; private set => dataCreacio = value; }
        public bool Validat { get => validat; set => validat = value; }
        public MitjaTransport MitjaTransport { get => mitjaTransport; set => mitjaTransport = value; }
        public Usuari UsuariCreador { get => usuariCreador; set => usuariCreador = value; }
        #endregion

        public override bool Equals(object? obj)
        {
            return obj is Trajecte trajecte &&
                   id == trajecte.id &&
                   nom.ToUpper() == trajecte.nom.ToUpper();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, nom.ToUpper());
        }
        

        public override string ToString()
        {
            string txt = Nom + " - " + mitjaTransport.Nom + " - creador: " + usuariCreador.Nom + ", creació: " + DataCreacio;
            if (validat) txt += ", validat";
            else txt += ", no validat";
            return txt;
        }

    }
}
