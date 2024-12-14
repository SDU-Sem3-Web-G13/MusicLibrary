using Backend.DataAccess.Interfaces;
using Backend.DataAccess.Dtos;

namespace Backend.DataAccess
{
    public class SqlAlbumRepository: IAlbumRepository
    {
        private readonly IDataSource _dataSource;

        public SqlAlbumRepository(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public void AddAlbum(int owner, byte[] cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks)
        {
            if(tracks == null)
            {
                tracks = [];
            }
            string sql = "INSERT INTO albums (a_owner, a_cover, a_name, a_releaseDate, a_artist, a_type, a_desc, a_tracks) VALUES (@owner, @cover, @albumName, @releaseDate, @artist, @type, @description, @tracks::text[])";
            _dataSource.ExecuteNonQuery(sql, ("@owner", owner), ("@cover", cover), ("@albumName", albumName), ("@releaseDate", releaseDate), ("@artist", artist), ("@type", type), ("@description", description), ("@tracks", tracks));
        }

        public void DeleteAlbum(int id)
        {
            string sql = "DELETE FROM albums WHERE a_id = @id";
            _dataSource.ExecuteNonQuery(sql, ("@id", id));
        }

        public void DeleteAllUserAlbums(int userId)
        {
            string sql = "DELETE FROM albums WHERE a_owner = @userId";
            _dataSource.ExecuteNonQuery(sql, ("@userId", userId));
        }

        public void ModifyAlbum(int id, int owner, byte[]? cover, string albumName, DateTime releaseDate, string artist, string type, string description, string[] tracks)
        {
            if (tracks == null)
            {
                tracks = [];
            }
            string sql = $"UPDATE albums SET a_owner = @owner,{(cover != null ?  " a_cover = @cover," : "")} a_name = @albumName, a_releaseDate = @releaseDate, a_artist = @artist, a_type = @type, a_desc = @description, a_tracks = @tracks::text[] WHERE a_id = @id";
            if(cover != null) _dataSource.ExecuteNonQuery(sql, ("@id", id), ("@owner", owner), ("@cover", cover), ("@albumName", albumName), ("@releaseDate", releaseDate), ("@artist", artist), ("@type", type), ("@description", description), ("@tracks", tracks));
            else _dataSource.ExecuteNonQuery(sql, ("@id", id), ("@owner", owner), ("@albumName", albumName), ("@releaseDate", releaseDate), ("@artist", artist), ("@type", type), ("@description", description), ("@tracks", tracks));
        }

        public List<IAlbumDto> GetAlbums(int userID)
        {
            string query = $"SELECT * FROM albums where a_owner = {userID}";

            List<IAlbumDto> albums = new List<IAlbumDto>();

            using (var reader = _dataSource.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    IAlbumDto album = new AlbumDto(
                        reader.GetString(3),
                        reader.GetDateTime(4),
                        reader.GetString(5),
                        reader.GetString(6),
                        reader.GetString(7),
                        (string[])reader.GetValue(8)
                    );
                    album.Id = reader.GetInt32(0);
                    album.CoverImage = (byte[])reader.GetValue(2);
                    album.OwnerId = reader.GetInt32(1);
                    albums.Add(album);
                }
            }

            return albums;
        }

        public IAlbumDto GetSingleAlbum(int albumId) {
            string query = $"SELECT * FROM albums WHERE a_id = {albumId}";
            using (var reader = _dataSource.ExecuteReader(query))
            {
                if (reader.Read())
                {
                    IAlbumDto album = new AlbumDto(
                        reader.GetString(3),
                        reader.GetDateTime(4),
                        reader.GetString(5),
                        reader.GetString(6),
                        reader.GetString(7),
                        (string[])reader.GetValue(8)
                        
                    );
                    album.Id = reader.GetInt32(0);
                    album.CoverImage = (byte[])reader.GetValue(2);
                    album.OwnerId = reader.GetInt32(1);
                    return album;
                }
            }
            throw new Exception("Album not found");
        }
    }
}
