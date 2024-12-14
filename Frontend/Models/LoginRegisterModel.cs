
using System.Text;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Models;

public interface ILoginRegisterModel
{
    bool EmailExists(string email);
    string GetFixedSalt();
    string ConvertToHex(string input);
    bool ValidateCredentials(string emailHash, string passwordHash);
    int GetUserId(string email);
    bool IsAdmin(int id);
    void AddUser(string firstName, string email);
    void AddUserCredentials(byte[] emailHash, byte[] passwordHash);
}

public class LoginRegisterModel: ILoginRegisterModel
{
    private readonly ILoginRegisterService _loginRegisterService;

    public LoginRegisterModel(ILoginRegisterService loginRegisterService)
    {
        _loginRegisterService = loginRegisterService;
    }

    
    public bool EmailExists(string email) {
        return _loginRegisterService.EmailExists(email);
    }

    public string GetFixedSalt() {
        return _loginRegisterService.GetFixedSalt();
    }

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

    public bool ValidateCredentials(string emailHash, string passwordHash)
    {
        return _loginRegisterService.ValidateCredentials(emailHash, passwordHash);
    }

    public int GetUserId(string email)
    {
        return _loginRegisterService.GetUserId(email);
    }

    public bool IsAdmin(int id)
    {
        return _loginRegisterService.IsAdmin(id);
    }

    public void AddUser(string firstName, string email)
    {
        _loginRegisterService.AddUser(firstName, email);
    }
    public void AddUserCredentials(byte[] emailHash, byte[] passwordHash)
    {
        _loginRegisterService.AddUserCredentials(emailHash, passwordHash);
    }
}