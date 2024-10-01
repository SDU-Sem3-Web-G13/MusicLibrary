using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorMusic.Models;
using RazorMusic.Models.Enums;

namespace RazorMusic.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public PageModel activePage { get; set; }

    private IndexDefaultUserModel indexDefaultUserModel { get; set; }


    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        
    }

    public void OnGet()
    {
        activePage = indexDefaultUserModel;        
    }
    public void OnPost()
    {
        
    }
}
