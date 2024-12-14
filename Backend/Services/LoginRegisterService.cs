using DotNetEnv;
using System.Text;
using Backend.DataAccess;
using System.Diagnostics;
using Backend.DataAccess.Interfaces;

namespace Backend.Services;
public interface ILoginRegisterService
{
    bool EmailExists(string email);
    bool ValidateCredentials(string emailHash, string passwordHash);
    string GetFixedSalt();
    void AddUserCredentials(byte[] emailHash, byte[] passwordHash);
    void AddUser(string firstName, string email);
    int GetUserId(string email);
    bool IsAdmin(int id);
}
public class LoginRegisterService: ILoginRegisterService
{
    private string fixedSalt { get; set; }

    private readonly IUserRepository userRepository;

    public LoginRegisterService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
        fixedSalt = LoadFixedSalt();
    }

    private string LoadFixedSalt()
    {
        string envPath = Path.Combine(AppContext.BaseDirectory, ".env");
        Env.Load(envPath);

        string base64Salt = Env.GetString("BCRYPT_SALT");
        return Encoding.UTF8.GetString(Convert.FromBase64String(base64Salt));
    }

    public bool EmailExists(string email)
    {
        return userRepository.EmailExists(email);
    }

    public bool ValidateCredentials(string emailHash, string passwordHash)
    {

        string dbPasswordHash = userRepository.GetHashedPassword(emailHash) ?? string.Empty;

        if (dbPasswordHash != null && dbPasswordHash == passwordHash)
        {
            return true;
        }
        return false;
    }

    public string GetFixedSalt()
    {
        return fixedSalt;
    }

    public void AddUserCredentials(byte[] emailHash, byte[] passwordHash)
    {
        userRepository.InsertUserCredentials(emailHash, passwordHash);
    }

    public int GetUserId(string email)
    {
        return userRepository.GetUserId(email);
    }

    public bool IsAdmin(int id)
    {
        return userRepository.IsAdmin(id);
    }

    public void AddUser(string firstName, string email)
    {
        userRepository.AddUser(firstName, email);
    }
}