using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorMusic.Pages;

public class PrivacyPolicyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyPolicyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}

