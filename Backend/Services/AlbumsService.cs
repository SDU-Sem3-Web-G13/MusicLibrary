using Backend.DataAccess;
using Backend.Services.ServiceDtos;
using Backend.DataAccess.Interfaces;

namespace Backend.Services
{
    public interface IAlbumsService
    {
        public List<IAlbumServiceDto> GetAlbums(int userId);
        public IAlbumServiceDto GetSingleAlbum(int albumId);
        public void DeleteAlbum(int id);
        public void DeleteAllUserAlbums(int userId);
        public void AddAlbum(int owner, byte[] cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks);
        public void ModifyAlbum(int id, int owner, byte[]? cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks);
    }

    public class AlbumsService: IAlbumsService
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumsService(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        
        public List<IAlbumServiceDto> GetAlbums(int userId)
        {
            var albums = _albumRepository.GetAlbums(userId);
            return albums.Select(album => { 
                var x = new AlbumServiceDto(
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
            }).ToList<IAlbumServiceDto>();
        }

        public IAlbumServiceDto GetSingleAlbum(int albumId) {
            var album = _albumRepository.GetSingleAlbum(albumId);
            var x = new AlbumServiceDto(
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

        public void DeleteAlbum(int id) {
            _albumRepository.DeleteAlbum(id);
        }

        public void DeleteAllUserAlbums(int userId)
        {
            _albumRepository.DeleteAllUserAlbums(userId);
        }

        public void AddAlbum(int owner, byte[] cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks)
        {
            _albumRepository.AddAlbum(owner, cover, albumName, releaseDate, artist, type, description, tracks);
        }


        public void ModifyAlbum(int id, int owner, byte[]? cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks)
        {
            _albumRepository.ModifyAlbum(id, owner, cover, albumName, releaseDate, artist, type, description, tracks);
        }
    }
}