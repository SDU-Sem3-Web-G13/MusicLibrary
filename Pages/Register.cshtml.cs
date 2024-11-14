using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DataAccess;
using Models.Services;
using System.Diagnostics;
using System.Text;

public class RegisterModel : PageModel
{
    [BindProperty]
    public User User { get; set; } = new User();

    [BindProperty]
    public string ErrorMessage { get; set; } = string.Empty;

    private readonly UserRepository _userRepository;
    private readonly UserCredentialsService _userCredentialsService;

    private readonly string FixedSalt;

    public RegisterModel()
    {
        _userRepository = new UserRepository();
        _userCredentialsService = new UserCredentialsService();
        FixedSalt = _userCredentialsService.GetFixedSalt();
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
       
        // Checking if email already exists 
        
        if (_userRepository.EmailExists(User.Email)) 
         {  
            ErrorMessage = "Email already exists.";
            return new JsonResult(new { success = false, errorMessage = ErrorMessage });
        }


        // Validate password
        if (!IsValidPassword(User.Password))
        {
            Debug.WriteLine("Password must be at least 8 characters");
            ErrorMessage = "Password must be at least 8 characters long.";
            return new JsonResult(new { success = false, errorMessage = ErrorMessage });
        }

        try
        {
            string emailHash = BCrypt.Net.BCrypt.HashPassword(User.Email, FixedSalt);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(User.Password, FixedSalt);

            byte[] emailHashBytes = System.Text.Encoding.UTF8.GetBytes(emailHash);
            byte[] passwordHashBytes = System.Text.Encoding.UTF8.GetBytes(passwordHash);

            _userRepository.AddUser(User.FirstName, User.Email);
            _userCredentialsService.AddUserCredentials(emailHashBytes, passwordHashBytes);

            return new JsonResult(new { success = true, redirectUrl = Url.Page("/Login") });
        }
        catch(Exception e)
        {
            return new JsonResult(new { success = false, errorMessage = ErrorMessage });

        }
        return Page();

    }

    private bool IsValidPassword(string password)
    {
        return password.Length >= 8; 
    }

    private byte[] HashPassword(string password)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}