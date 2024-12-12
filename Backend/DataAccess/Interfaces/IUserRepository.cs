using Backend.Models;

namespace Backend.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        public void AddUser(string name, string mail);
        public void InsertUserCredentials(byte[] mailHash, byte[] passHash);
        public void ModifyUser(int id, string name, string mail);
        public void ModifyUserCredentials(byte[] mailHash, byte[] passHash);
        public void DeleteUser(int id);
        public void DeleteUserCredentials(byte[] mailHash);
        public List<UserModel> GetUsers();
        public UserModel GetSingleUser(int id);
        public int GetUserId(string email);
        public string? GetHashedPassword(string hashedEmailHex);
        public bool EmailExists(string email);
        public bool IsAdmin(int? userId);
    }
}