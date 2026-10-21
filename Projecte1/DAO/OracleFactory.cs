
using System;

namespace DAO
{
    public class OracleFactory
    {
        public static void getUOW(
            string connectionString,
            Action<UnitOfWork> action)
        {
            using (var uow = new UnitOfWork(
                new ConnexioBd(connectionString)))
            {
                try
                {
                    uow.beginTransaction();

                    action(uow);

                    uow.Commit();
                }
                catch
                {
                    uow.Rollback();
                    throw;
                }
            }
        }
    }
}
