using Model.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IDAO
{
    public interface IDAOMitjaTransport
    {
        public ObservableCollection<MitjaTransport> GetMitjansTransport();
    }
}
