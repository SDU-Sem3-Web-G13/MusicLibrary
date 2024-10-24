using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    internal class UserRepository
    {
        private readonly DbAccess dbAccess;

        public UserRepository()
        {
            dbAccess = new DbAccess();
        }

        public void AddUser(string name, string mail)
        {
            string sql = "INSERT INTO users (name, mail) VALUES (@name, @mail)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@mail", mail));

        }

        public void DeleteUser(int id)
        {
            string sql = "DELETE FROM users WHERE id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public void ModifyUser(int id, string name, string mail)
        {
            string sql = "UPDATE users SET name = @name, mail = @mail WHERE id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id), ("@name", name), ("@mail", mail));
        }

        public void AddUserCredentials(byte mail, byte password)
        {
            string sql = "INSERT INTO usercredentials (mail, password) VALUES (@mail, @password)";
            dbAccess.ExecuteNonQuery(sql, ("@mail", mail), ("@password", password));
        }

        public void ModifyUserCredintials(int id, byte mail, byte password)
        {
            string sql = "UPDATE usercredentials SET mail = @mail, password = @password WHERE id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id), ("@mail", mail), ("@password", password));
        }

        public void DeleteUserCredintials(int id)
        {
            string sql = "DELETE FROM usercredentials WHERE id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }
    }

}
