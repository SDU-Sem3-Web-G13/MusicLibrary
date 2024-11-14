using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DataAccess;
using Models.Services;

public class LoginModel : PageModel
{
    [BindProperty]
    public User User { get; set; } = new User(); 

    public string ErrorMessage { get; set; } = string.Empty;

    [BindProperty]
    public string? Email { get; set; }
    [BindProperty]
    public string? Password { get; set; }

    private readonly UserCredentialsService userCredentialsService;
    private readonly string fixedSalt;

    public LoginModel()
    {
        userCredentialsService = new UserCredentialsService();
        fixedSalt = userCredentialsService.GetFixedSalt();
    }

    public void OnGet()
    {
        // Initialization logic if needed
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            ErrorMessage = "Email and password are required.";
            return new JsonResult(new { success = false, errorMessage = ErrorMessage });
        }

        if (string.IsNullOrEmpty(User.Email) || string.IsNullOrEmpty(User.Password))
        {
            ErrorMessage = "Email and password are required.";
            return new JsonResult(new { success = false, errorMessage = ErrorMessage });
        }

        string emailHash = BCrypt.Net.BCrypt.HashPassword(User.Email, fixedSalt);
        string hashedEmailHex = userCredentialsService.ConvertToHex(emailHash);
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(User.Password, fixedSalt);
        string hashedPasswordHex = userCredentialsService.ConvertToHex(passwordHash);

        if (userCredentialsService.ValidateCredentials(hashedEmailHex, hashedPasswordHex))
        {
            return new JsonResult(new { success = true, redirectUrl = Url.Page("/AlbumsView") });
        }
        else
        {
            ErrorMessage = "Invalid email or password.";
        }

        return new JsonResult(new { success = false, errorMessage = ErrorMessage });
    }



}
