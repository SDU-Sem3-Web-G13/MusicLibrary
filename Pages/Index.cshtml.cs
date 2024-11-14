using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.Enums;

namespace RazorMusic.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        
    }

    public void OnGet()
    {
        ClearSession();
    }

    public void OnPost()
    {
        
    }

    private void ClearSession()
    {
        HttpContext.Session.Clear();
        HttpContext.Session.SetInt32("IsLoggedIn", 0);
        HttpContext.Session.SetString("UserEmail", "");
    }
}
