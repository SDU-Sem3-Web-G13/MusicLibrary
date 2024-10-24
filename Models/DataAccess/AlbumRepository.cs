using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess
{
    internal class AlbumRepository
    {
        private readonly DbAccess dbAccess;

        public AlbumRepository()
        {
            dbAccess = new DbAccess();
        }

        public void AddAlbum(int owner , byte cover ,string albumName , DateTime releaseDate,string artist, string type, string description, string[] tracks)
        {
            string sql = "INSERT INTO albums (owner, cover, albumName, releaseDate, artist, type, description, tracks) VALUES (@owner, @cover, @albumName, @releaseDate, @artist, @type, @description, @tracks)";
            dbAccess.ExecuteNonQuery(sql, ("@owner", owner), ("@cover", cover), ("@albumName", albumName), ("@releaseDate", releaseDate), ("@artist", artist), ("@type", type), ("@description", description), ("@tracks", tracks));

        }

        public void DeleteAlbum(int id)
        {
            string sql = "DELETE FROM albums WHERE id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public void ModifyAlbum(int id, int owner, byte cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks)
        {
            string sql = "UPDATE albums SET owner = @owner, cover = @cover, albumName = @albumName, releaseDate = @releaseDate, artist = @artist, type = @type, description = @description, tracks = @tracks WHERE id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id), ("@owner", owner), ("@cover", cover), ("@albumName", albumName), ("@releaseDate", releaseDate), ("@artist", artist), ("@type", type), ("@description", description), ("@tracks", tracks));
        }
    }
}
