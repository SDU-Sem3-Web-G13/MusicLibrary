using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DataAccess;

namespace RazorMusic.Pages;

public class AdminViewModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;
    private readonly UserRepository userRepository = new UserRepository();

    public AdminViewModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        ValidateSessionStorage();
    }

    private void ValidateSessionStorage() {
        if (HttpContext.Session.GetInt32("IsLoggedIn") != 1) {
            Response.Redirect("/Login");
        } 
        var userId = HttpContext.Session.GetInt32("userId");
        if(userId == null || !userRepository.IsAdmin(userId.Value)) {
            Response.Redirect("/Index");
        }
    }
}

