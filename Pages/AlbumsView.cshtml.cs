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
            foreach(var album in albumRepository.GetAlbums()) {
                Albums.Add(album);
            }
        }
    }
}
