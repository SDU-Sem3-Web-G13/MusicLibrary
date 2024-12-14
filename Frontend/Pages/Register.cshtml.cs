using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Backend.Models;
using System.Diagnostics;

using Frontend.Models;

public class RegisterViewModel : PageModel
{
    [BindProperty]
    public LoginUser LoginUser { get; set; } = new LoginUser();

    [BindProperty]
    public string ErrorMessage { get; set; } = string.Empty;

    private readonly ILoginRegisterModel model;

    private readonly string FixedSalt;

    public RegisterViewModel(ILoginRegisterModel model)
    {
        this.model = model;
        FixedSalt = model.GetFixedSalt();
    }

    public IActionResult OnPost()
    {
        try
        {
            string emailHash = BCrypt.Net.BCrypt.HashPassword(LoginUser.Email, FixedSalt);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(LoginUser.Password, FixedSalt);

            byte[] emailHashBytes = System.Text.Encoding.UTF8.GetBytes(emailHash);
            byte[] passwordHashBytes = System.Text.Encoding.UTF8.GetBytes(passwordHash);

            var userExists = model.EmailExists(LoginUser.Email);
            if(userExists)
            {
                ErrorMessage = "User already exists";
                return Page();
            }

            model.AddUser(LoginUser.FirstName, LoginUser.Email);
            model.AddUserCredentials(emailHashBytes, passwordHashBytes);

            HttpContext.Session.SetInt32("IsLoggedIn", 1);
            var userId = model.GetUserId(LoginUser.Email);
            HttpContext.Session.SetInt32("userId", userId);
            HttpContext.Session.SetInt32("IsAdmin", model.IsAdmin(userId) ? 1 : 0);

            LoginUser = new LoginUser();
            return RedirectToPage("AlbumsView");
        }
        catch(Exception e)
        {
            Debug.WriteLine(e.Message);
        }
        return Page();
    }
}