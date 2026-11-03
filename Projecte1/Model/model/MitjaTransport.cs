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

        // Constructor per mitjans de transport de la BD
        public MitjaTransport(int id, string nom)
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
