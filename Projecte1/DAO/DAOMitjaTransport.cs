using IDAO;
using Model.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DAO
{
    public class DAOMitjaTransport : IDAOMitjaTransport
    {
        private readonly ConnexioBd _connexio;

        public DAOMitjaTransport(ConnexioBd connexio)
        {
            _connexio = connexio;
        }

        public ObservableCollection<MitjaTransport> GetMitjansTransport()
        {
            throw new NotImplementedException();
        }
    }
}
