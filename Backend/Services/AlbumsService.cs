using Backend.DataAccess;
using Backend.DataAccess.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public interface IAlbumsService
    {
        public List<AlbumModel> GetAlbums(int userId);
        public AlbumModel GetSingleAlbum(int albumId);
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

        
        public List<AlbumModel> GetAlbums(int userId)
        {
            return _albumRepository.GetAlbums(userId);
        }

        public AlbumModel GetSingleAlbum(int albumId) {
            return _albumRepository.GetSingleAlbum(albumId);
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