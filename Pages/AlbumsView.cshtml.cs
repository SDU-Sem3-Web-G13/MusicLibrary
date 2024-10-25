using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorMusic.Models;

namespace RazorMusic.Pages
{
    public class AlbumsViewModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public List<AlbumModel> Albums { get; set; } = new List<AlbumModel>();
        public void OnGet()
        {
            GetUserAlbums();
        }

        private void GetUserAlbums() {
            Albums.Add(new AlbumModel(
                "The Dark Side of the Moon",
                new DateTime(1973, 3, 1),
                "Pink Floyd",
                "LP",
                "The Dark Side of the Moon is the eighth studio album by the English rock band Pink Floyd, released on 1 March 1973 by Harvest Records.",
                new string[] {
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
            Albums.Add(new AlbumModel(
                "Abbey Road",
                new DateTime(1969, 9, 26),
                "The Beatles",
                "LP",
                "Abbey Road is the eleventh studio album by the English rock band the Beatles, released on 26 September 1969 by Apple Records.",
                new string[] {
                    "Come Together",
                    "Something",
                    "Maxwell's Silver Hammer",
                    "Oh! Darling",
                    "Octopus's Garden",
                    "I Want You (She's So Heavy)",
                    "Here Comes the Sun",
                    "Because",
                    "You Never Give Me Your Money",
                    "Sun King",
                    "Mean Mr. Mustard",
                    "Polythene Pam",
                    "She Came in Through the Bathroom Window",
                    "Golden Slumbers",
                    "Carry That Weight",
                    "The End",
                    "Her Majesty"
                }
            ));
            Albums.Add(new AlbumModel(
                "The Wall",
                new DateTime(1979, 11, 30),
                "Pink Floyd",
                "LP",
                "The Wall is the eleventh studio album by the English rock band Pink Floyd, released on 30 November 1979 by Harvest and Columbia Records.",
                new string[] {
                    "In the Flesh?",
                    "The Thin Ice",
                    "Another Brick in the Wall, Part 1",
                    "The Happiest Days of Our Lives",
                    "Another Brick in the Wall, Part 2",
                    "Mother",
                    "Goodbye Blue Sky",
                    "Empty Spaces",
                    "Young Lust",
                    "One of My Turns",
                    "Don't Leave Me Now",
                    "Another Brick in the Wall, Part 3",
                    "Goodbye Cruel World",
                    "Hey You",
                    "Is There Anybody Out There?",
                    "Nobody Home",
                    "Vera"
                }
            ));
        }
    }
}
