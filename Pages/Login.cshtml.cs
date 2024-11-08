using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DataAccess;

public class LoginModel : PageModel
{
    [BindProperty]
    public User User { get; set; } = new User(); 

    public string ErrorMessage { get; set; }
    [BindProperty]
    public string Email { get; set; }
    [BindProperty]
    public string Password { get; set; }

    private readonly UserRepository _userRepository;

    public LoginModel(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void OnGet()
    {
        // Initialization logic if needed
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Email and password are required.";
            return Page();
        }
        return Page(); // direct to album page
    }

}
