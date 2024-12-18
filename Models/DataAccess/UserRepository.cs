﻿using Models;

namespace Models.DataAccess
{
    internal class UserRepository
    {
        private readonly DbAccess dbAccess;

        public UserRepository()
        {
            dbAccess = new DbAccess();
        }

        #region Add Methods

        public void AddUser(string name, string mail)
        {
            string sql = "INSERT INTO USERS (U_NAME, U_MAIL) VALUES (@name, @mail)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@mail", mail));

        }

        public void InsertUserCredentials(byte[] mailHash, byte[] passHash)
        {
            string sql = "INSERT INTO USER_CREDENTIALS (UMAIL_HASH, UPASS_HASH) VALUES (@mailHash, @passHash)";
            dbAccess.ExecuteNonQuery(sql, ("@mailHash", mailHash), ("@passHash", passHash));
        }


        #endregion

        #region Modify Methods

        public void ModifyUser(int id, string name, string mail)
        {
            string sql = "UPDATE USERS SET U_NAME = @name, U_MAIL = @mail WHERE U_ID = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id), ("@name", name), ("@mail", mail));
        }



        public void ModifyUserCredentials(byte[] mailHash, byte[] passHash)
        {
            string sql = "UPDATE USER_CREDENTIALS SET UPASS_HASH = @passHash WHERE UMAIL_HASH = @mailHash";
            dbAccess.ExecuteNonQuery(sql, ("@mailHash", mailHash), ("@passHash", passHash));
        }

        #endregion

        #region Delete methods
        public void DeleteUser(int id)
        {
            string sql = "DELETE FROM USERS WHERE U_ID = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        

        public void DeleteUserCredentials(byte[] mailHash)
        {
            string sql = "DELETE FROM USER_CREDENTIALS WHERE UMAIL_HASH = @mailHash";
            dbAccess.ExecuteNonQuery(sql, ("@mailHash", mailHash));
        }
        #endregion

        #region Get Methods 
        public List<UserModel> GetUsers()
        {
            string query = "SELECT * FROM USERS";
            List<UserModel> users = new List<UserModel>();
            using (var cmd = dbAccess.dbDataSource.CreateCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserModel user = new UserModel(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetBoolean(3)
                        );
                        users.Add(user);
                    }
                }
            }
            return users;
        }

        public int GetUserId(string email) 
        {
            string query = $"SELECT u_id FROM USERS WHERE U_MAIL = '{email}' LIMIT 1";
            using(var cmd = dbAccess.dbDataSource.CreateCommand(query))
            {
                using(var reader = cmd.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            return 0;
        }

        public bool VerifyUserCredentials(string email, string password)
        {
            string query = $"SELECT * FROM USER_CREDENTIALS WHERE umail_hash = {email}";
            bool isValidUser = false;
            /*
            using (var cmd = dbAccess.dbDataSource.CreateCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    var databaseEmail = reader.GetFieldValue<byte[]>(0);
                    var databasePassword = reader.GetFieldValue<byte[]>(1);
                    if (BCrypt.Net.BCrypt.Verify(email, databaseEmail) && BCrypt.Net.BCrypt.Verify(password, databasePassword))
                    {
                        isValidUser = true;
                    }
                }
            }
            */
            return isValidUser;
        }

        public string? GetHashedPassword(string hashedEmailHex)
        {
            string sql = "SELECT upass_hash FROM user_credentials WHERE umail_hash = decode(@hashedEmailHex, 'hex')";

            using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
            {
                cmd.Parameters.Add("@hashedEmailHex", NpgsqlTypes.NpgsqlDbType.Varchar).Value = hashedEmailHex;

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        byte[] passwordHashBytes = (byte[])reader["upass_hash"];

                        return BitConverter.ToString(passwordHashBytes).Replace("-", "").ToLower();
                    }
                }
            }
            return null;
        }

        public bool EmailExists(string email)
        {
            string sql = "SELECT u_mail FROM users";
            List<string> emails = new List<string>();

            using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string user = reader.GetString(0);


                        emails.Add(user);
                    }
                }
            }
            if (emails.Contains(email))
            {
                return true;
            }
            return false;
        }

        public bool IsAdmin(int? userId) {
            if (userId == null) return false;
            string sql = $"SELECT U_ISADMIN FROM USERS WHERE U_ID = {userId}";
            using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetBoolean(0);
                    }
                }
            }
            return false;
        }


        #endregion



    }

}



