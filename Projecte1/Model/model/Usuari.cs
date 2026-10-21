using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Model.model
{
    public class Usuari : INotifyPropertyChanged
    {
        private int id;
        private string email;
        private string nom;
        private string password;
        private TipusUsuari tipus;
        private DateTime dataRegistre;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Usuari(int id, string email, string nom, string password, TipusUsuari tipus, DateTime dataRegistre)
        {
            Id = id;
            Email = email;
            Nom = nom;
            Password = password;
            Tipus = tipus;
            DataRegistre = dataRegistre;
        }
        public Usuari(int id, string email, string password, TipusUsuari tipus, DateTime dataRegistre)
        {
            Id = id;
            Email = email;
            Nom = null;
            Password = password;
            Tipus = tipus;
            DataRegistre = dataRegistre;
        }

        public int Id { get => id; set => id = value; }
        public string Email { get => email; set => email = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Password { get => password; set => password = value; }
        public TipusUsuari Tipus { get => tipus; set => tipus = value; }
        public DateTime DataRegistre { get => dataRegistre; set => dataRegistre = value; }

        public override string ToString()
        {
            return Nom + ": " + Email + " - " + Tipus + ", registre: " + DataRegistre;
        }

        public override bool Equals(object? obj)
        {
            return obj is Usuari usuari &&
                   id == usuari.id &&
                   email.ToUpper() == usuari.email.ToUpper();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, email.ToUpper());
        }
    }

}
