using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Model.model
{
    public class MitjaTransport
    {
        private int id;
        private string nom;

        public MitjaTransport(int id, string nom)
        {
            Id = id;
            Nom = nom;
        }

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }

        public override bool Equals(object? obj)
        {
            return obj is MitjaTransport transport &&
                   id == transport.id &&
                   nom.ToUpper() == transport.nom.ToUpper();
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
