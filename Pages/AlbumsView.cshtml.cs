using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.DataAccess;

namespace RazorMusic.Pages
{
    public class AlbumsViewModel : PageModel
    {
        private readonly AlbumRepository albumRepository = new AlbumRepository();
        
        [BindProperty(SupportsGet = true)]
        public List<AlbumModel> Albums { get; set; } = new List<AlbumModel>();
        public void OnGet()
        {
            GetUserAlbums();
        }

        private void GetUserAlbums() {
            Albums.Clear();
            foreach(var album in albumRepository.GetAlbums()) {
                Albums.Add(album);
            }
        }

        public IActionResult OnGetDeleteAlbum(int albumId) {
            albumRepository.DeleteAlbum(albumId);
            GetUserAlbums();
            return new JsonResult(new { success = true });
        }

        public IActionResult OnGetAddAlbum(string albumName, DateTime releaseDate, string artist, string albumType, string description, string[] tracks) {
            albumRepository.AddAlbum(1, new byte[1], albumName, releaseDate, artist, albumType, description, tracks);
            GetUserAlbums();
            return new JsonResult(new { success = true });
        }

        public IActionResult OnGetEditAlbum(int albumId, string albumName, DateTime releaseDate, string artist, string albumType, string description, string[] tracks) {
            return new JsonResult(new { success = true });
        }
    }
}
