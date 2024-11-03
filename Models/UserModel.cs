
namespace Models
{
    internal class UserModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Mail { get; private set; }

        internal UserModel(int id,string name, string mail)
        {
            this.Id = id;
            this.Name = name;
            this.Mail = mail;
        }
    }
}
