using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Backend.Models;
using Frontend.Models;

namespace RazorMusic.Pages
{
    public class AlbumsViewModel : PageModel
    {
        private IAlbumsModel model;

        [BindProperty(SupportsGet = true)]
        public List<AlbumModel> Albums { get; set; } = new List<AlbumModel>();

        [BindProperty]
        public AlbumInputModel InputAlbum { get; set; } = null!;

        public AlbumsViewModel(IAlbumsModel albumsModel)
        {
            model = albumsModel;
        }

        public void OnGet()
        {
            ValidateSessionStorage();
            GetUserAlbums();
        }

        private void ValidateSessionStorage() {
            if (HttpContext.Session.GetInt32("IsLoggedIn") != 1) {
                Response.Redirect("/Login");
            } 
        }

        private void GetUserAlbums() {
            var userId = HttpContext.Session.GetInt32("userId");
            Albums = model.GetAlbums(userId ?? 0);
        }

#region ActionResults

        public IActionResult OnGetGetAlbum(int albumId) {
            var album = model.GetSingleAlbum(albumId);
            return new JsonResult(new { success = true, album });
        }

        public IActionResult OnGetDeleteAlbum(int albumId) {
            model.DeleteAlbum(albumId);
            GetUserAlbums();
            return new JsonResult(new { success = true });
        }

        public IActionResult OnGetClearInputAlbum() {
            InputAlbum = new AlbumInputModel();
            return new JsonResult(new { success = true });
        }
        public IActionResult OnPostAddAlbum() {            
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null) return new JsonResult(new { success = false });

            model.AddAlbum(userId ?? 0, InputAlbum.CoverImage, InputAlbum.AlbumName, InputAlbum.ReleaseDate, InputAlbum.Artist, InputAlbum.AlbumType, InputAlbum.Description, InputAlbum.Tracks);
            GetUserAlbums();
            return new JsonResult(new { success = true });
        }

        public IActionResult OnPostEditAlbum() {
            var userId = HttpContext.Session.GetInt32("userId");
            if(userId == null) return new JsonResult(new { success = false });
            
            model.ModifyAlbum(InputAlbum.AlbumId, userId ?? 0 , InputAlbum.CoverImage, InputAlbum.AlbumName, InputAlbum.ReleaseDate, InputAlbum.Artist, InputAlbum.AlbumType, InputAlbum.Description, InputAlbum.Tracks);
            GetUserAlbums();
            return new JsonResult(new { success = true });
        }

        public IActionResult OnGetRetrieveAlbumCover(int albumId) {
            var album = model.GetSingleAlbum(albumId);
            if (album.CoverImage == null) return new JsonResult(new { success = false });

            var base64Image = album.GetCoverImageBase64();
            return new JsonResult(new { success = true, coverImage = base64Image });
        }
    }

#endregion

    public class AlbumInputModel
    {   
        public int AlbumId { get; set; }
        public int AlbumOwnerId { get; set; } 
        public string AlbumName { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public string Artist { get; set; } = null!;
        public string AlbumType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string[] Tracks { get; set; } = null!;
        public IFormFile CoverImage { get; set; } = null!;
    }
}

