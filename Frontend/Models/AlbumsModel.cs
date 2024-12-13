using Backend.Models;
using Backend.Services;

namespace Frontend.Models
{
    public class AlbumsModel
    {
        private readonly IAlbumsService _albumsService;

        public AlbumsModel(IAlbumsService albumsService)
        {
            _albumsService = albumsService;
        }

        public List<AlbumModel> GetAlbums(int userId) 
        {
            return _albumsService.GetAlbums(userId);
        }

        public AlbumModel GetSingleAlbum(int albumId) 
        {
            return _albumsService.GetSingleAlbum(albumId);
        }

        public void DeleteAlbum(int id) 
        {
            _albumsService.DeleteAlbum(id);
        }

        public void DeleteAllUserAlbums(int userId) 
        {
            _albumsService.DeleteAllUserAlbums(userId);
        }
        
        public void ModifyAlbum(int id, int owner, IFormFile? cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks) 
        {
            var coverImageBytes = GetCoverImageBytes(cover);
            _albumsService.ModifyAlbum(id, owner, coverImageBytes, albumName, releaseDate, artist, type, description, tracks);
        }

        public void AddAlbum(int owner, IFormFile? cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks) 
        {
            byte[] coverImageBytes = GetCoverImageBytes(cover) ?? [];
            _albumsService.AddAlbum(owner, coverImageBytes, albumName, releaseDate, artist, type, description, tracks);
        }

        private byte[]? GetCoverImageBytes(IFormFile? cover) {
            byte[]? coverImageBytes;
            if (cover != null) {
                using (var memoryStream = new MemoryStream()) {
                    cover.CopyTo(memoryStream);
                    coverImageBytes = memoryStream.ToArray();
                }
            } else {
                coverImageBytes = null;
            }

            return coverImageBytes;
        }
    }
}