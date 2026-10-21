using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace Model.model
{
    public class Parada : INotifyPropertyChanged
    {
        private int ordre;
        private Trajecte trajecte;
        private string nom;
        private string horaPas;
        private Estacio estacio;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Parada(int ordre, Trajecte trajecte, string nom, string horaPas, Estacio estacio)
        {
            Ordre = ordre;
            Trajecte = trajecte;
            Nom = nom;
            HoraPas = horaPas;
            Estacio = estacio;
        }

        #region propietats

        public int Ordre { get => ordre; set => ordre = value; }
        public Trajecte Trajecte { get => trajecte; set => trajecte = value; }
        public string Nom { get => nom; set => nom = value; }
        public string HoraPas { get => horaPas; 
            set 
            {
                if (validarHoraPas(value)) horaPas = value;
                else throw new Exception("Hora de pas incorrecta");
            } 
        }
        public Estacio Estacio { get => estacio; set => estacio = value; }

        #endregion

        public override string ToString()
        {
            return trajecte.Nom + " - Parada: " + Nom + " - Estació: " + estacio.Nom + " hora de pas: " + HoraPas;
        }

        #region validacions

        public static bool validarHoraPas(string horaPas)
        {
            Regex rgx = new Regex("^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
            bool ok = rgx.IsMatch(horaPas);
            return ok;
        }

        public override bool Equals(object? obj)
        {
            return obj is Parada parada &&
                   ordre == parada.ordre &&
                   EqualityComparer<Trajecte>.Default.Equals(trajecte, parada.trajecte);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ordre, trajecte);
        }

        #endregion
    }
}
