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

    private readonly UserRepository _userRepository;
    private readonly UserCredentialsService _userCredentialsService;

    private string fixedSalt { get; set; }

    public RegisterModel()
    {
        _userRepository = new UserRepository();
        _userCredentialsService = new UserCredentialsService();
        fixedSalt = _userCredentialsService.GetFixedSalt();
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Checking if email already exists 
        
        //if (_userRepository.EmailExists(User.Email)) 
         //{
          //   ModelState.AddModelError("User.Email", "Email already exists.");
           //  return Page();
         //}


        // Validate password
        if (!IsValidPassword(User.Password))
        {
            ModelState.AddModelError("User.Password", "Password must be at least 8 characters long.");
            return Page();
        }

        try
        {
            string emailHash = BCrypt.Net.BCrypt.HashPassword(User.Email, fixedSalt);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(User.Password, fixedSalt);

            byte[] emailHashBytes = System.Text.Encoding.UTF8.GetBytes(emailHash);
            byte[] passwordHashBytes = System.Text.Encoding.UTF8.GetBytes(passwordHash);

            _userRepository.AddUser(User.FirstName, User.Email);
            _userCredentialsService.AddUserCredentials(emailHashBytes, passwordHashBytes);

            return RedirectToPage("/Login"); // we can chage to which page we want to redirect
        }
        catch(Exception e)
        {
            Debug.WriteLine(e.Message);
            
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