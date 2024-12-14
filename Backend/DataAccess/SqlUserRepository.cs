using Backend.DataAccess.Interfaces;
using Backend.DataAccess.Dtos;

namespace Backend.DataAccess
{
    public class SqlUserRepository: IUserRepository
    {
        private readonly IDataSource _dataSource;

        public SqlUserRepository(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        #region Add Methods

        public void AddUser(string name, string mail)
        {
            string sql = "INSERT INTO USERS (U_NAME, U_MAIL) VALUES (@name, @mail)";
            _dataSource.ExecuteNonQuery(sql, ("@name", name), ("@mail", mail));
        }

        public void InsertUserCredentials(byte[] mailHash, byte[] passHash)
        {
            string sql = "INSERT INTO USER_CREDENTIALS (UMAIL_HASH, UPASS_HASH) VALUES (@mailHash, @passHash)";
            _dataSource.ExecuteNonQuery(sql, ("@mailHash", mailHash), ("@passHash", passHash));
        }


        #endregion

        #region Modify Methods

        public void ModifyUser(int id, string name, string mail)
        {
            string sql = "UPDATE USERS SET U_NAME = @name, U_MAIL = @mail WHERE U_ID = @id";
            _dataSource.ExecuteNonQuery(sql, ("@id", id), ("@name", name), ("@mail", mail));
        }



        public void ModifyUserCredentials(byte[] mailHash, byte[] passHash)
        {
            string sql = "UPDATE USER_CREDENTIALS SET UPASS_HASH = @passHash WHERE UMAIL_HASH = @mailHash";
            _dataSource.ExecuteNonQuery(sql, ("@mailHash", mailHash), ("@passHash", passHash));
        }

        #endregion

        #region Delete methods
        public void DeleteUser(int id)
        {
            string sql = "DELETE FROM USERS WHERE U_ID = @id";
            _dataSource.ExecuteNonQuery(sql, ("@id", id));
        }

        

        public void DeleteUserCredentials(byte[] mailHash)
        {
            string sql = "DELETE FROM USER_CREDENTIALS WHERE UMAIL_HASH = @mailHash";
            _dataSource.ExecuteNonQuery(sql, ("@mailHash", mailHash));
        }
        #endregion

        #region Get Methods 
        public List<IUserDto> GetUsers()
        {
            string query = "SELECT * FROM USERS";
            List<IUserDto> users = new List<IUserDto>();
            using (var reader = _dataSource.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    IUserDto user = new UserDto(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetBoolean(3)
                    );
                    users.Add(user);
                }
            }
            return users;
        }

        public IUserDto GetSingleUser(int id)
        {
            string query = $"SELECT * FROM USERS WHERE U_ID = {id}";
            using (var reader = _dataSource.ExecuteReader(query))
            {
                if (reader.Read())
                {
                    return new UserDto(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetBoolean(3)
                    );
                }
            }
            return null!;
        }

        public int GetUserId(string email) 
        {
            string query = $"SELECT u_id FROM USERS WHERE U_MAIL = '{email}' LIMIT 1";
            using(var reader = _dataSource.ExecuteReader(query))
            {
                if(reader.Read())
                {
                    return reader.GetInt32(0);
                }
            }
            return 0;
        }

        public string? GetHashedPassword(string hashedEmailHex)
        {
            string query = "SELECT upass_hash FROM user_credentials WHERE umail_hash = decode(@hashedEmailHex, 'hex')";

            var parameters = new (string, object)[] { ("@hashedEmailHex", hashedEmailHex) };
            using (var reader = _dataSource.ExecuteReader(query, parameters))
            {
                if (reader.Read())
                {
                    byte[] passwordHashBytes = (byte[])reader["upass_hash"];
                    return BitConverter.ToString(passwordHashBytes).Replace("-", "").ToLower();
                }
            }
            return null;
        }

        public bool EmailExists(string email)
        {
            string query = "SELECT u_mail FROM users";
            List<string> emails = new List<string>();

            using (var reader = _dataSource.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    string user = reader.GetString(0);
                    emails.Add(user);
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
            string query = $"SELECT U_ISADMIN FROM USERS WHERE U_ID = {userId}";
            using (var reader = _dataSource.ExecuteReader(query))
            {
                if (reader.Read())
                {
                    return reader.GetBoolean(0);
                }
            }
            return false;
        }
#endregion
    }
}



