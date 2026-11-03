using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Model.model
{
    public class Ciutat
    {
        private int id;
        private string nom;

        // Constructor per ciutats de la BD
        public Ciutat(int id, string nom)
        {
            Id = id;
            Nom = nom;
        }

        #region propietats
        public int Id { get => id; private set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        #endregion

        public override bool Equals(object? obj)
        {
            return obj is Ciutat ciutat &&
                   id == ciutat.id &&
                   nom.ToUpper() == ciutat.nom.ToUpper();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, nom.ToUpper());
        }

        public override string ToString()
        {
            return nom;
        }

    }
}
