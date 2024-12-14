using System;
using System.Data.Common;
using DotNetEnv;
using Npgsql;
using System.Diagnostics;
using Backend.DataAccess.Interfaces;
using System.Data;

namespace Backend.DataAccess
{
    public class PostgresDataSource: IDataSource
    {
        private readonly NpgsqlConnection _connection;

        public PostgresDataSource(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public static NpgsqlConnection CreateConnection()
        {
            string envPath = Path.Combine(AppContext.BaseDirectory, ".env");
            Env.Load(envPath);

            var user = Env.GetString("POSTGRES_USER");
            var password = Env.GetString("POSTGRES_PASSWORD");
            var dbName = Env.GetString("POSTGRES_DB");
            var port = Env.GetString("POSTGRES_PORT");
            var servicename = "";
            if(Env.GetString("ASPNETCORE_ENVIRONMENT") == "Release") {
                servicename = Env.GetString("SERVICE_NAME");
            } else {
                servicename = "localhost";
            }

            var connectionString = $"Host={servicename};Port={port};Database={dbName};User Id={user};Password={password};";
            return new NpgsqlConnection(connectionString);
        }

        public IDbCommand CreateCommand(string query)
        {
            return new NpgsqlCommand(query, _connection);
        }

        public int ExecuteNonQuery(string sql, params (string, object)[] parameters)
        {
            using (var cmd = CreateCommand(sql))
            {
                foreach (var param in parameters)
                {
                    ((NpgsqlCommand)cmd).Parameters.AddWithValue(param.Item1, param.Item2);
                }
                
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                try
                {
                    return cmd.ExecuteNonQuery();
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public IDataReader ExecuteReader(string query, params (string, object)[] parameters)
        {
            using (var cmd = CreateCommand(query))
            {
                foreach (var param in parameters)
                {
                    ((NpgsqlCommand)cmd).Parameters.AddWithValue(param.Item1, param.Item2);
                }
                
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                try
                {
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch
                {
                    _connection.Close();
                    throw;
                }
            }
        }
    }
}
