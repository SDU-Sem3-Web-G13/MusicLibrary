using System;
using System.Data.Common;
using DotNetEnv;
using Npgsql;
using System.Diagnostics;

namespace Models.DataAccess
{
    public class DbAccess
    {
        private string user { get; set; }
        private string password { get; set; }
        private string dbName { get; set; }
        private string port { get; set; }
        private string connectionString { get; set; }
        private string servicename { get; set; }
        public NpgsqlDataSource dbDataSource { get; set; }

        public DbAccess()
        {
            string envPath = Path.Combine(AppContext.BaseDirectory, ".env");
            Env.Load(envPath);

            user = Env.GetString("POSTGRES_USER");
            password = Env.GetString("POSTGRES_PASSWORD");
            dbName = Env.GetString("POSTGRES_DB");
            port = Env.GetString("POSTGRES_PORT");

            if(Env.GetString("ASPNETCORE_ENVIRONMENT") == "Release") {
                servicename = Env.GetString("SERVICE_NAME");
            } else {
                servicename = "localhost";
            }
            connectionString = $"Host={servicename};Port={port};Database={dbName};User Id={user};Password={password};";
            

            dbDataSource = NpgsqlDataSource.Create(connectionString);
        }

        public void ExecuteNonQuery(string sql, params (string, object)[] parameters)
        {
            try
            {
                using (var cmd = dbDataSource.CreateCommand(sql))
                {
                    foreach (var (name, value) in parameters)
                    {
                        cmd.Parameters.AddWithValue(name, value);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public int GetId(string sql, string name, string value)
        {
            try
            {
                using (var cmd = dbDataSource.CreateCommand(sql))
                {
                    cmd.Parameters.AddWithValue(name, value);
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        return reader.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        



    }
}
