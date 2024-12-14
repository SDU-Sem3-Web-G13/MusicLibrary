using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace Backend.DataAccess.Interfaces
{
    public interface IDataSource
    {
        IDbCommand CreateCommand(string query);
        int ExecuteNonQuery(string sql, params (string, object)[] parameters);
        IDataReader ExecuteReader(string query, params (string, object)[] parameters);
    }
}