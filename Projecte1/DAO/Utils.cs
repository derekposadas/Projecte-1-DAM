using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    internal class Utils
    {
        public static void CrearParametre(DbCommand query, object? value, string name, DbType type)
        {
            if (query is Oracle.ManagedDataAccess.Client.OracleCommand oracleCommand)
            {
                oracleCommand.BindByName = true;
            }

            DbParameter p = query.CreateParameter();
            p.Value = value ?? DBNull.Value;
            p.ParameterName = name;
            p.DbType = type;
            query.Parameters.Add(p);
        }
    }
}
