using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.DataAccess;
using System.Text;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;

namespace RazorMusic.Pages
{
    public class AlbumsViewModel : PageModel
    {
        private readonly AlbumRepository albumRepository = new AlbumRepository();
        
        [BindProperty(SupportsGet = true)]
        public List<AlbumModel> Albums { get; set; } = new List<AlbumModel>();

        [BindProperty]
        public AlbumInputModel InputAlbum { get; set; } = null!;

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
        public FileResult OnPostExportCsv()
{
    ValidateSessionStorage();
    GetUserAlbums();

    var csv = new StringBuilder();
    csv.AppendLine("Album's Id,Album's Name,Release Date,Artist,Album's Type,Description,Tracks");
    foreach (var album in Albums)
    {
        csv.AppendLine($"{album.Id},{album.AlbumName},{album.ReleaseDate:yyyy-MM-dd},{album.Artist},{album.AlbumType},{album.Description},{string.Join("|", album.Tracks)}");
    }
    byte[] csvBytes = Encoding.UTF8.GetBytes(csv.ToString());

    string? userPassword = HttpContext.Session.GetString("userPassword");
    if (string.IsNullOrEmpty(userPassword))
    {
        Console.WriteLine("User password not found in session.");
        return File(new byte[0], "application/zip", "error.zip");
    }

    using var memoryStream = new MemoryStream();
    using (var zipOutputStream = new ZipOutputStream(memoryStream))
    {
        zipOutputStream.SetLevel(9); 
        zipOutputStream.Password = userPassword; 

        var csvEntry = new ZipEntry("exported_albums.csv")
        {
            DateTime = DateTime.Now,
            Size = csvBytes.Length
        };
        zipOutputStream.PutNextEntry(csvEntry);
        zipOutputStream.Write(csvBytes, 0, csvBytes.Length);
        zipOutputStream.CloseEntry();

        foreach (var album in Albums)
        {
            if (album.CoverImage != null && album.CoverImage.Length > 0)
            {
                var imageEntry = new ZipEntry($"cover_images/album_{album.Id}.jpg")
                {
                    DateTime = DateTime.Now,
                    Size = album.CoverImage.Length
                };
                zipOutputStream.PutNextEntry(imageEntry);
                zipOutputStream.Write(album.CoverImage, 0, album.CoverImage.Length);
                zipOutputStream.CloseEntry();
            }
        }
        zipOutputStream.Finish(); 
    }

    //memoryStream.Seek(0, SeekOrigin.Begin); // Reset the memory stream position to the beginning
    return File(memoryStream.ToArray(), "application/zip", "exported_albums_with_covers.zip");
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
}

