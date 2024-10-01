using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorMusic.Models;
using RazorMusic.Models.Enums;

namespace RazorMusic.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public List<AlbumModel> Albums {get; set;} = new List<AlbumModel>();
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        
    }

    public void OnGet()
    {
        //this.Albums = new List<AlbumModel>();
        this.Albums.Add(new AlbumModel(
            "The Dark Side of the Moon",
            new DateTime(1973, 3, 1),
            "Pink Floyd",
            AlbumTypes.LP.ToString(),
            "The Dark Side of the Moon is the eighth studio album by the English rock band Pink Floyd, released on 1 March 1973 by Harvest Records.",
            new string[10] {
                "Speak to Me",
                "Breathe",
                "On the Run",
                "Time",
                "The Great Gig in the Sky",
                "Money",
                "Us and Them",
                "Any Colour You Like",
                "Brain Damage",
                "Eclipse"
            }
        ));
    }
    public void OnPost()
    {
        var album = new AlbumModel(
            Request.Form["AlbumName"],
            DateTime.Parse(Request.Form["ReleaseDate"]),
            Request.Form["Artist"],
            Request.Form["AlbumType"],
            Request.Form["Description"],
            new string[10] {
                Request.Form["Track1"],
                Request.Form["Track2"],
                Request.Form["Track3"],
                Request.Form["Track4"],
                Request.Form["Track5"],
                Request.Form["Track6"],
                Request.Form["Track7"],
                Request.Form["Track8"],
                Request.Form["Track9"],
                Request.Form["Track10"]
            }
        );
        this.Albums.Add(album);
    }
}
