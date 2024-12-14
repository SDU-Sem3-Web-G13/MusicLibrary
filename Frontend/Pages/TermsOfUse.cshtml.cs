using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorMusic.Pages;

public class TermsOfUseModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public TermsOfUseModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}

