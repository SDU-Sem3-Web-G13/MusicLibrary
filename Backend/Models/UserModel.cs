namespace Backend.Models
{
    public class UserModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Mail { get; private set; }
        public bool IsAdmin { get; private set; }

        public UserModel(int id,string name, string mail, bool isAdmin)
        {
            this.Id = id;
            this.Name = name;
            this.Mail = mail;
            this.IsAdmin = isAdmin;
        }
    }
}
