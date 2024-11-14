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

    private readonly UserRepository _userRepository;
    private readonly UserCredentialsService userCredentialsService;
    private readonly string fixedSalt;

    public LoginModel()
    {
        _userRepository = new UserRepository();
        userCredentialsService = new UserCredentialsService();
        fixedSalt = userCredentialsService.GetFixedSalt();
    }

    public void OnGet()
    {
        // Initialization logic if needed
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(User.Email) || string.IsNullOrEmpty(User.Password))
        {
            ErrorMessage = "Email and password are required.";
            return Page();
        }

        string emailHash = BCrypt.Net.BCrypt.HashPassword(User.Email, fixedSalt);
        string hashedEmailHex = userCredentialsService.ConvertToHex(emailHash);
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(User.Password, fixedSalt);
        string hashedPasswordHex = userCredentialsService.ConvertToHex(passwordHash);

        if (userCredentialsService.ValidateCredentials(hashedEmailHex, hashedPasswordHex))
        {
            return RedirectToPage("/AlbumsView");
        }
        else
        {
            ErrorMessage = "Invalid email or password.";
        }

        return Page(); // direct to album page
    }

}
