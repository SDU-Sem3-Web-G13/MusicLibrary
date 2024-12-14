using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services.ServiceDtos
{
    public interface IUserServiceDto: IServiceDto
    {
        int Id { get; }
        string Name { get; }
        string Mail { get; }
        bool IsAdmin { get; }
    }

    public class UserServiceDto: IUserServiceDto
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Mail { get; private set; }
        public bool IsAdmin { get; private set; }

        public UserServiceDto(int id, string name, string mail, bool isAdmin)
        {
            this.Id = id;
            this.Name = name;
            this.Mail = mail;
            this.IsAdmin = isAdmin;
        }
    }
}