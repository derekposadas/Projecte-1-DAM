
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class ConnexioBd : DbContext
    {
        public ConnexioBd(string connectionString)
            : base(
                new DbContextOptionsBuilder<ConnexioBd>()
                    .UseOracle(connectionString)
                    .Options)
        {
        }
    }
}
