using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Backend.Models;
using Frontend.Models;

public class LoginViewModel : PageModel
{
    [BindProperty]
    public LoginUser LoginUser { get; set; } = new LoginUser(); 

    public string? ErrorMessage { get; set; } = null;

    private readonly ILoginRegisterModel model;
    private readonly string fixedSalt;

    public LoginViewModel(ILoginRegisterModel model)
    {
        this.model = model; 
        fixedSalt = model.GetFixedSalt();
    }

    public IActionResult OnPost()
    {
        if(!model.EmailExists(LoginUser.Email)) return Page();

        string emailHash = BCrypt.Net.BCrypt.HashPassword(LoginUser.Email, fixedSalt);
        string hashedEmailHex = model.ConvertToHex(emailHash);
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(LoginUser.Password, fixedSalt);
        string hashedPasswordHex = model.ConvertToHex(passwordHash);

        if (model.ValidateCredentials(hashedEmailHex, hashedPasswordHex))
        {
            HttpContext.Session.SetInt32("IsLoggedIn", 1);
            var userId = model.GetUserId(LoginUser.Email);
            HttpContext.Session.SetInt32("userId", userId);
            HttpContext.Session.SetInt32("IsAdmin", model.IsAdmin(userId) ? 1 : 0);
            ErrorMessage = null;

            LoginUser = new LoginUser();
            return RedirectToPage("AlbumsView");
        }
        else
        {
            LoginUser = new LoginUser();
            ErrorMessage = "Invalid password.";
        }

        return Page(); 
    }

}
