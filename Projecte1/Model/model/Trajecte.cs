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
        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public DateTime DataCreacio { get => dataCreacio; set => dataCreacio = value; }
        public bool Validat { get => validat; set => validat = value; }
        public MitjaTransport MitjaTransport { get => mitjaTransport; set => mitjaTransport = value; }
        public Usuari UsuariCreador { get => usuariCreador; set => usuariCreador = value; }

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
        #endregion

        public override string ToString()
        {
            string txt = Nom + " - " + mitjaTransport.Nom + " - creador: " + usuariCreador.Nom + ", creació: " + DataCreacio;
            if (validat) txt += ", validat";
            else txt += ", no validat";
            return txt;
        }

    }
}
