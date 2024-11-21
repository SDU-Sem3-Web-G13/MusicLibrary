using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DataAccess;
using Models.Services;
using Microsoft.AspNetCore.Http;

public class LoginModel : PageModel
{
    [BindProperty]
    public LoginUser LoginUser { get; set; } = new LoginUser(); 

    public string? ErrorMessage { get; set; } = null;

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
        
    }

    public IActionResult OnPost()
    {
        if(!_userRepository.EmailExists(LoginUser.Email)) return Page();

        string emailHash = BCrypt.Net.BCrypt.HashPassword(LoginUser.Email, fixedSalt);
        string hashedEmailHex = userCredentialsService.ConvertToHex(emailHash);
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(LoginUser.Password, fixedSalt);
        string hashedPasswordHex = userCredentialsService.ConvertToHex(passwordHash);

        if (userCredentialsService.ValidateCredentials(hashedEmailHex, hashedPasswordHex))
        {
            HttpContext.Session.SetInt32("IsLoggedIn", 1);
            var userId = _userRepository.GetUserId(LoginUser.Email);
            HttpContext.Session.SetInt32("userId", userId);
            HttpContext.Session.SetInt32("IsAdmin", _userRepository.IsAdmin(userId) ? 1 : 0);
            ErrorMessage = null;
            return RedirectToPage("AlbumsView");
        }
        else
        {
            ErrorMessage = "Invalid password.";
        }

        return Page(); 
    }

}
