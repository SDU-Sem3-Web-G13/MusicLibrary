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

        [BindProperty]
        public AlbumInputModel InputAlbum { get; set; } = null!;

        [BindProperty(SupportsGet = true)]
        public string OrderByVariable { get; set; } = "Tittle";

        [BindProperty(SupportsGet = true)]
        public string OrderBy { get; set; } = "Ascending";

        public void OnGet()
        {
            ValidateSessionStorage();
            GetUserAlbums();
            SortAlbums();
        }


        private void ValidateSessionStorage() {
            if (HttpContext.Session.GetInt32("IsLoggedIn") != 1) {
                Response.Redirect("/Login");
            } 
        }


        private void GetUserAlbums() {
            Albums.Clear();
            var userId = HttpContext.Session.GetInt32("userId");
            foreach(var album in albumRepository.GetAlbums(userId ?? 0)) {
                Albums.Add(album);
            }
        }

        public IActionResult OnGetGetAlbum(int albumId) {
            var album = albumRepository.GetSingleAlbum(albumId);
            return new JsonResult(new { success = true, album });
        }


        public IActionResult OnGetDeleteAlbum(int albumId) {
            albumRepository.DeleteAlbum(albumId);
            GetUserAlbums();
            return new JsonResult(new { success = true });
        }

        public IActionResult OnGetClearInputAlbum() {
            InputAlbum = new AlbumInputModel();
            return new JsonResult(new { success = true });
        }
        public IActionResult OnPostAddAlbum() {

            byte[] coverImageBytes;
            if (InputAlbum.CoverImage != null) {
                using (var memoryStream = new MemoryStream()) {
                    InputAlbum.CoverImage.CopyTo(memoryStream);
                    coverImageBytes = memoryStream.ToArray();
                }
            } else {
                coverImageBytes = new byte[1];
            }
            
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null) return new JsonResult(new { success = false });
            albumRepository.AddAlbum(userId ?? 0, coverImageBytes, InputAlbum.AlbumName, InputAlbum.ReleaseDate, InputAlbum.Artist, InputAlbum.AlbumType, InputAlbum.Description, InputAlbum.Tracks);
            GetUserAlbums();
            return new JsonResult(new { success = true });
        }

        public IActionResult OnPostEditAlbum() {
            byte[]? coverImageBytes;
            if (InputAlbum.CoverImage != null) {
                using (var memoryStream = new MemoryStream()) {
                    InputAlbum.CoverImage.CopyTo(memoryStream);
                    coverImageBytes = memoryStream.ToArray();
                }
            } else {
                coverImageBytes = null;
            }
            var userId = HttpContext.Session.GetInt32("userId");

            if(userId == null) return new JsonResult(new { success = false });
            
            albumRepository.ModifyAlbum(InputAlbum.AlbumId, userId ?? 0 , coverImageBytes, InputAlbum.AlbumName, InputAlbum.ReleaseDate, InputAlbum.Artist, InputAlbum.AlbumType, InputAlbum.Description, InputAlbum.Tracks);
            GetUserAlbums();
            return new JsonResult(new { success = true });
        }

        public IActionResult OnGetRetrieveAlbumCover(int albumId) {
            var album = albumRepository.GetSingleAlbum(albumId);
            if (album.CoverImage == null) return new JsonResult(new { success = false });

            var base64Image = Convert.ToBase64String(album.CoverImage);
            return new JsonResult(new { success = true, coverImage = base64Image });
        }

        private void SortAlbums()
        {
            switch (OrderByVariable)
            {
                case "Tittle":
                    Albums = OrderBy == "Ascending"
                        ? Albums.OrderBy(a => a.AlbumName).ToList()
                        : Albums.OrderByDescending(a => a.AlbumName).ToList();
                    break;
                case "Author":
                    Albums = OrderBy == "Ascending"
                        ? Albums.OrderBy(a => a.Artist).ToList()
                        : Albums.OrderByDescending(a => a.Artist).ToList();
                    break;
                case "ReleaseDate":
                    Albums = OrderBy == "Ascending"
                        ? Albums.OrderBy(a => a.ReleaseDate).ToList()
                        : Albums.OrderByDescending(a => a.ReleaseDate).ToList();
                    break;
            }
        }
    }

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

