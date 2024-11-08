using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DataAccess;
using System.Text;

public class RegisterModel : PageModel
{
    [BindProperty]
    public User User { get; set; } = new User();

    private readonly UserRepository _userRepository;

    public RegisterModel(UserRepository userRepository)
    {
        _userRepository = userRepository;
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
        if (_userRepository.EmailExists(User.Email)) 
         {
             ModelState.AddModelError("User.Email", "Email already exists.");
             return Page();
         }

        // Validate password
        if (!IsValidPassword(User.Password))
        {
            ModelState.AddModelError("User.Password", "Password must be at least 8 characters long.");
            return Page();
        }

         byte[] passHash = HashPassword(User.Password);

        _userRepository.AddUser(User.FirstName, User.Email);
        _userRepository.AddUserCredentials(User.Email, passHash);

        return RedirectToPage("/Login"); // we can chage to which page we want to redirect
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