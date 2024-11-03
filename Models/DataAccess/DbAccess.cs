﻿using System;
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
        public NpgsqlDataSource dbDataSource { get; set; }

        public DbAccess()
        {
            string envPath = @"../Data/_Setup/.env";
            Env.Load(envPath);

            user = Env.GetString("POSTGRES_USER");
            password = Env.GetString("POSTGRES_PASSWORD");
            dbName = Env.GetString("POSTGRES_DB");
            port = Env.GetString("POSTGRES_PORT");
            //connectionString = $"Host=localhost;Port={port};Database={dbName};User Id={user};Password={password};";
            connectionString = "Host=localhost;Port=5434;Database=docker;User Id=docker;Password=docker;";

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