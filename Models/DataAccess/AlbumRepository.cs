using System;
using RazorMusic.Models;

namespace DataAccess
{
    internal class AlbumRepository
    {
        private readonly DbAccess dbAccess;

        public AlbumRepository()
        {
            dbAccess = new DbAccess();
        }

        public void AddAlbum(int owner, byte[] cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks)
        {
            string sql = "INSERT INTO albums (a_owner, a_cover, a_name, a_releaseDate, a_artist, a_type, a_desc, a_tracks) VALUES (@owner, @cover, @albumName, @releaseDate, @artist, @type, @description, @tracks)";
            dbAccess.ExecuteNonQuery(sql, ("@owner", owner), ("@cover", cover), ("@albumName", albumName), ("@releaseDate", releaseDate), ("@artist", artist), ("@type", type), ("@description", description), ("@tracks", tracks));
        }

        public void DeleteAlbum(string albumName)
        {
            string idSql = $"SELECT a_id FROM albums WHERE a_name = @name";
            int id = dbAccess.GetId(idSql, "@name", albumName);
            string sql = "DELETE FROM albums WHERE a_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public void ModifyAlbum(int id, int owner, byte[] cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks)
        {
            string sql = "UPDATE albums SET a_owner = @owner, a_cover = @cover, a_name = @albumName, a_releaseDate = @releaseDate, a_artist = @artist, a_type = @type, a_desc = @description, a_tracks = @tracks WHERE a_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id), ("@owner", owner), ("@cover", cover), ("@albumName", albumName), ("@releaseDate", releaseDate), ("@artist", artist), ("@type", type), ("@description", description), ("@tracks", tracks));
        }

        public List<AlbumModel> GetAlbums()
        {
            string query = "SELECT * FROM albums";

            List<AlbumModel> albums = new List<AlbumModel>();

            using (var cmd = dbAccess.dbDataSource.CreateCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AlbumModel album = new AlbumModel()
                        {
                            AlbumName = reader.GetString(3),
                            ReleaseDate = reader.GetDateTime(5),
                            Artist = reader.GetString(6),
                            AlbumType = reader.GetString(7),
                            Description = reader.GetString(8),
                            Tracks = reader.GetString(9).Split(", ")
                        };
                        albums.Add(album);
                    }
                }
            }

            return albums;
        }
    }
}
