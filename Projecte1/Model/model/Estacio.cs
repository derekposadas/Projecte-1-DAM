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

        public Estacio(int id, Ciutat ciutat, string nom, double latitud, double longitud)
        {
            Id = id;
            Ciutat = ciutat;
            Nom = nom;
            Latitud = latitud;
            Longitud = longitud;
        }

        public int Id { get => id; set => id = value; }
        public Ciutat Ciutat { get => ciutat; set => ciutat = value; }
        public string Nom { get => nom; set => nom = value; }
        public double Latitud { get => latitud; set => latitud = value; }
        public double Longitud { get => longitud; set => longitud = value; }


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
