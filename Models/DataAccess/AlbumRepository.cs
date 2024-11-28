using System;
using Models;
using System.Diagnostics;

namespace Models.DataAccess
{
    public class AlbumRepository
    {
        private readonly DbAccess dbAccess;

        public AlbumRepository()
        {
            dbAccess = new DbAccess();
        }

        public void AddAlbum(int owner, byte[] cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks)
        {
            if(tracks == null)
            {
                tracks = new string[0];
            }
            string sql = "INSERT INTO albums (a_owner, a_cover, a_name, a_releaseDate, a_artist, a_type, a_desc, a_tracks) VALUES (@owner, @cover, @albumName, @releaseDate, @artist, @type, @description, @tracks::text[])";
            dbAccess.ExecuteNonQuery(sql, ("@owner", owner), ("@cover", cover), ("@albumName", albumName), ("@releaseDate", releaseDate), ("@artist", artist), ("@type", type), ("@description", description), ("@tracks", tracks));
        }

        public void DeleteAlbum(int id)
        {
            string sql = "DELETE FROM albums WHERE a_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteAllUserAlbums(int userId)
        {
            string sql = "DELETE FROM albums WHERE a_owner = @userId";
            dbAccess.ExecuteNonQuery(sql, ("@userId", userId));
        }

        public void ModifyAlbum(int id, int owner, byte[]? cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks)
        {
            if (tracks == null)
            {
                tracks = new string[0];
            }
            string sql = $"UPDATE albums SET a_owner = @owner,{(cover != null ?  " a_cover = @cover," : "")} a_name = @albumName, a_releaseDate = @releaseDate, a_artist = @artist, a_type = @type, a_desc = @description, a_tracks = @tracks::text[] WHERE a_id = @id";
            if(cover != null) dbAccess.ExecuteNonQuery(sql, ("@id", id), ("@owner", owner), ("@cover", cover), ("@albumName", albumName), ("@releaseDate", releaseDate), ("@artist", artist), ("@type", type), ("@description", description), ("@tracks", tracks));
            else dbAccess.ExecuteNonQuery(sql, ("@id", id), ("@owner", owner), ("@albumName", albumName), ("@releaseDate", releaseDate), ("@artist", artist), ("@type", type), ("@description", description), ("@tracks", tracks));
        }

        public List<AlbumModel> GetAlbums(int userID)
        {
            string query = $"SELECT * FROM albums where a_owner = {userID}";

            List<AlbumModel> albums = new List<AlbumModel>();

            using (var cmd = dbAccess.dbDataSource.CreateCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AlbumModel album = new AlbumModel(
                            
                            reader.GetString(3),
                            reader.GetDateTime(4),
                            reader.GetString(5),
                            reader.GetString(6),
                            reader.GetString(7),
                            reader.GetFieldValue<string[]>(8)
                        );
                        album.Id = reader.GetInt32(0);
                        album.CoverImage = reader.GetFieldValue<byte[]>(2);
                        album.OwnerId = reader.GetInt32(1);
                        albums.Add(album);
                    }
                }
            }

            return albums;
        }

        public AlbumModel GetSingleAlbum(int albumId) {
            string query = $"SELECT * FROM albums WHERE a_id = {albumId}";
            using (var cmd = dbAccess.dbDataSource.CreateCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        AlbumModel album = new AlbumModel(
                            reader.GetString(3),
                            reader.GetDateTime(4),
                            reader.GetString(5),
                            reader.GetString(6),
                            reader.GetString(7),
                            reader.GetFieldValue<string[]>(8)
                            
                        );
                        album.Id = reader.GetInt32(0);
                        album.CoverImage = reader.GetFieldValue<byte[]>(2);
                        album.OwnerId = reader.GetInt32(1);
                        return album;
                    }
                }
            }
            throw new Exception("Album not found");
        }
        public (int totalAlbums, int totalArtists, int totalTracks, int totalGenres) GetStatistics()
        {
            string query = @"
                SELECT 
                    COUNT(*) AS totalAlbums, 
                    COUNT(DISTINCT a_artist) AS totalArtists,
                    SUM(array_length(a_tracks, 1)) AS totalTracks,
                    COUNT(DISTINCT a_type) AS totalGenres
                FROM albums";

            using (var cmd = dbAccess.dbDataSource.CreateCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (
                            reader.GetInt32(0),  // totalAlbums
                            reader.GetInt32(1),  // totalArtists
                            reader.IsDBNull(2) ? 0 : reader.GetInt32(2), // totalTracks
                            reader.GetInt32(3)   // totalGenres
                        );
                    }
                }
            }
            throw new Exception("Failed to retrieve statistics.");
        }
    }
}
