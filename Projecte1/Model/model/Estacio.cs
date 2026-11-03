using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model.model
{
    public class Estacio : INotifyPropertyChanged
    {
        private int id;
        private Ciutat ciutat;
        private string nom;
        private double latitud;
        private double longitud;

        public event PropertyChangedEventHandler? PropertyChanged;

        // Constructor per estacions noves
        public Estacio(Ciutat ciutat, string nom, double latitud, double longitud)
        {
            Ciutat = ciutat;
            Nom = nom;
            Latitud = latitud;
            Longitud = longitud;
        }

        // Constructor per ciutats de la BD
        public Estacio(int id, Ciutat ciutat, string nom, double latitud, double longitud)
        {
            Id = id;
            Ciutat = ciutat;
            Nom = nom;
            Latitud = latitud;
            Longitud = longitud;
        }

        #region propietats
        public int Id { get => id; private set => id = value; }
        public Ciutat Ciutat { get => ciutat; set => ciutat = value; }
        public string Nom { get => nom; set => nom = value; }
        public double Latitud { get => latitud; set => latitud = value; }
        public double Longitud { get => longitud; set => longitud = value; }
        #endregion

        public override bool Equals(object? obj)
        {
            return obj is Estacio estacio &&
                   id == estacio.id &&
                   ciutat.Id == estacio.ciutat.Id &&
                   nom.ToUpper() == estacio.nom.ToUpper();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, ciutat.Id, nom.ToUpper());
        }

        public override string ToString()
        {
            return ciutat.Nom + " - " + nom + " cords: " + latitud + ", " + longitud;
        }
    }
}
