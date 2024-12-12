using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.DataAccess.Interfaces
{
    public interface IAlbumRepository
    {
        public void AddAlbum(int owner, byte[] cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks);
        public void DeleteAlbum(int id);
        public void DeleteAllUserAlbums(int userId);
        public void ModifyAlbum(int id, int owner, byte[]? cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks);
        public List<AlbumModel> GetAlbums(int userID);
        public AlbumModel GetSingleAlbum(int albumId);
    }
}