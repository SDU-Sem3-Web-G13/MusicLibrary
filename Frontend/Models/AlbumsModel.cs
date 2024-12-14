using Frontend.Objects;
using Backend.Services;

namespace Frontend.Models
{
    public interface IAlbumsModel
    {
        List<AlbumModel> GetAlbums(int userId);
        AlbumModel GetSingleAlbum(int albumId);
        void DeleteAlbum(int id);
        void DeleteAllUserAlbums(int userId);
        void ModifyAlbum(int id, int owner, IFormFile? cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks);
        void AddAlbum(int owner, IFormFile? cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks);
    }

    public class AlbumsModel: IAlbumsModel
    {
        private readonly IAlbumsService _albumsService;

        public AlbumsModel(IAlbumsService albumsService)
        {
            _albumsService = albumsService;
        }

        public List<AlbumModel> GetAlbums(int userId) 
        {
            var serviceDtos = _albumsService.GetAlbums(userId);
            var albumModels = serviceDtos.Select(dto => {
                var x = new AlbumModel(
                    dto.AlbumName,
                    dto.ReleaseDate,
                    dto.Artist,
                    dto.AlbumType,
                    dto.Description,
                    dto.Tracks
                );
                x.Id = dto.Id;
                x.OwnerId = dto.OwnerId;
                x.CoverImage = dto.CoverImage;
                return x;
            }).ToList();
            return albumModels;
        }

        public AlbumModel GetSingleAlbum(int albumId) 
        {
            var album = _albumsService.GetSingleAlbum(albumId);
            var x = new AlbumModel(
                album.AlbumName,
                album.ReleaseDate,
                album.Artist,
                album.AlbumType,
                album.Description,
                album.Tracks
            );
            x.Id = album.Id;
            x.OwnerId = album.OwnerId;
            x.CoverImage = album.CoverImage;
            return x;
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