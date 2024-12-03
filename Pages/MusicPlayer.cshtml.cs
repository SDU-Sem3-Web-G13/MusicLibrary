using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorMusic.Pages
{
    public class MusicPlayerModel : PageModel
    {

        private readonly ILogger<MusicPlayerModel> _logger;

        public MusicPlayerModel(ILogger<MusicPlayerModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
    
}