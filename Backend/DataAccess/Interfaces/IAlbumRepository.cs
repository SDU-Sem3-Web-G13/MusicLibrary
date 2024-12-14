using Backend.DataAccess.Dtos;

namespace Backend.DataAccess.Interfaces
{
    public interface IAlbumRepository
    {
        public void AddAlbum(int owner, byte[] cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks);
        public void DeleteAlbum(int id);
        public void DeleteAllUserAlbums(int userId);
        public void ModifyAlbum(int id, int owner, byte[]? cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks);
        public List<IAlbumDto> GetAlbums(int userID);
        public IAlbumDto GetSingleAlbum(int albumId);
    }
}