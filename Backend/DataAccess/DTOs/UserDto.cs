using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DataAccess.Dtos
{
    public interface IUserDto: IDto
    {
        int Id { get; }
        string Name { get; }
        string Mail { get; }
        bool IsAdmin { get; }
    }

    public class UserDto: IUserDto
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Mail { get; private set; }
        public bool IsAdmin { get; private set; }

        public UserDto(int id, string name, string mail, bool isAdmin)
        {
            this.Id = id;
            this.Name = name;
            this.Mail = mail;
            this.IsAdmin = isAdmin;
        }
    }
}