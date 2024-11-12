using DotNetEnv;
using System.Text;
using Models.DataAccess;

namespace Models.Services;
public class UserCredentialsService
{
    private string fixedSalt { get; set; }

    private readonly UserRepository userRepository = new UserRepository();
    public UserCredentialsService()
    {
        string envPath = "./Models/DataAccess/_Setup/.env";
        Env.Load(envPath);

        string base64Salt = Env.GetString("BCRYPT_SALT");
        fixedSalt = Encoding.UTF8.GetString(Convert.FromBase64String(base64Salt));
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

    // For converting the hashed password to hex for database querying.
    public string ConvertToHex(string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        StringBuilder hex = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes)
        {
            hex.AppendFormat("{0:x2}", b);
        }
        return hex.ToString();
    }

    public void AddUserCredentials(byte[] emailHash, byte[] passwordHash)
    {
        userRepository.InsertUserCredentials(emailHash, passwordHash);
    }
}