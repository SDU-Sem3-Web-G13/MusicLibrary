using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorMusic.Pages;

public class TermsOfUseModel : PageModel
{
    private readonly ILogger<TermsOfUseModel> _logger;

    public TermsOfUseModel(ILogger<TermsOfUseModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}

