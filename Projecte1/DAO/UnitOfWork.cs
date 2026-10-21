using IDAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public class UnitOfWork : IDisposable
    {
        private readonly ConnexioBd _connexio;

        public IDAOCiutat DAOCiutats { get; }
        public IDAOEstacio DAOEstacions { get; }
        public IDAOMitjaTransport DAOMitjaTransports { get; }
        public IDAOParada DAOParades { get; }
        public IDAOTrajecte DAOTrajectes { get; }
        public IDAOUsuari DAOUsuaris { get; }

        public UnitOfWork(ConnexioBd connexio)
        {
            _connexio = connexio;
            DAOCiutats = new DAOCiutat(_connexio);
            DAOEstacions = new DAOEstacio(_connexio);
            DAOMitjaTransports = new DAOMitjaTransport(_connexio);
            DAOParades = new DAOParada(_connexio);
            DAOTrajectes = new DAOTrajecte(_connexio);
            DAOUsuaris = new DAOUsuari(_connexio);
        }

        public void beginTransaction() => _connexio.Database.BeginTransaction();
        public void Commit() => _connexio.Database.CurrentTransaction?.Commit();
        public void Rollback() => _connexio.Database.CurrentTransaction?.Rollback();
        public void Dispose() => _connexio.Dispose();
    }
}
