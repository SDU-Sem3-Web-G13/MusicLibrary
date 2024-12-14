namespace Backend.Tests.RepositoryTests
{
    public class SqlAlbumRepositoryTests
    {
        private readonly Mock<IDataSource> _mockDataSource;
        private readonly SqlAlbumRepository _repository;

        public SqlAlbumRepositoryTests()
        {
            _mockDataSource = new Mock<IDataSource>();
            _repository = new SqlAlbumRepository(_mockDataSource.Object);
        }

        [Fact]
        public void AddAlbum_ShouldCallExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int owner = 1;
            byte[] cover = [1, 2, 3];
            string albumName = "Test Album";
            DateTime releaseDate = DateTime.Now;
            string artist = "Test Artist";
            string type = "Test Type";
            string description = "Test Description";
            string[] tracks = ["Track1", "Track2"];

            // Act
            _repository.AddAlbum(owner, cover, albumName, releaseDate, artist, type, description, tracks);

            // Assert
            _mockDataSource.Verify(ds => ds.ExecuteNonQuery(
                It.Is<string>(sql => sql.Contains("INSERT INTO albums")),
                It.IsAny<(string, object)[]>()
            ), Times.Once);
        }

        [Fact]
        public void DeleteAlbum_ShouldCallExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int albumId = 1;

            // Act
            _repository.DeleteAlbum(albumId);

            // Assert
            _mockDataSource.Verify(ds => ds.ExecuteNonQuery(
                It.Is<string>(sql => sql.Contains("DELETE FROM albums WHERE a_id = @id")),
                It.IsAny<(string, object)[]>()
            ), Times.Once);
        }

        [Fact]
        public void DeleteAllUserAlbums_ShouldCallExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int userId = 1;

            // Act
            _repository.DeleteAllUserAlbums(userId);

            // Assert
            _mockDataSource.Verify(ds => ds.ExecuteNonQuery(
                It.Is<string>(sql => sql.Contains("DELETE FROM albums WHERE a_owner = @userId")),
                It.IsAny<(string, object)[]>()
            ), Times.Once);
        }

        [Fact]
        public void ModifyAlbum_ShouldCallExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int albumId = 1;
            int owner = 1;
            byte[] cover = [1, 2, 3];
            string albumName = "Test Album";
            DateTime releaseDate = DateTime.Now;
            string artist = "Test Artist";
            string type = "Test Type";
            string description = "Test Description";
            string[] tracks = ["Track1", "Track2"];

            // Act
            _repository.ModifyAlbum(albumId, owner, cover, albumName, releaseDate, artist, type, description, tracks);

            // Assert
            _mockDataSource.Verify(ds => ds.ExecuteNonQuery(
                It.Is<string>(sql => sql.Contains("UPDATE albums SET")),
                It.IsAny<(string, object)[]>()
            ), Times.Once);
        }

        [Fact]
        public void GetAlbums_ShouldReturnListOfAlbums()
        {
            // Arrange
            int userId = 1;
            var mockReader = new Mock<IDataReader>();
            mockReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(false);
            mockReader.Setup(r => r.GetInt32(0)).Returns(1);
            mockReader.Setup(r => r.GetInt32(1)).Returns(userId);
            mockReader.Setup(r => r.GetString(3)).Returns("Test Album");
            mockReader.Setup(r => r.GetDateTime(4)).Returns(DateTime.Now);
            mockReader.Setup(r => r.GetString(5)).Returns("Test Artist");
            mockReader.Setup(r => r.GetString(6)).Returns("Test Type");
            mockReader.Setup(r => r.GetString(7)).Returns("Test Description");
            mockReader.Setup(r => r.GetValue(8)).Returns(new string[] { "Track1", "Track2" });
            mockReader.Setup(r => r.GetValue(2)).Returns(new byte[] { 1, 2, 3 });

            _mockDataSource.Setup(ds => ds.ExecuteReader(It.IsAny<string>())).Returns(mockReader.Object);

            // Act
            var albums = _repository.GetAlbums(userId);

            // Assert
            albums.Should().HaveCount(1);
            albums[0].OwnerId.Should().Be(userId);
        }

        [Fact]
        public void GetSingleAlbum_ShouldReturnAlbum()
        {
            // Arrange
            int albumId = 1;
            var mockReader = new Mock<IDataReader>();
            mockReader.Setup(r => r.Read()).Returns(true);
            mockReader.Setup(r => r.GetInt32(0)).Returns(albumId);
            mockReader.Setup(r => r.GetInt32(1)).Returns(1);
            mockReader.Setup(r => r.GetString(3)).Returns("Test Album");
            mockReader.Setup(r => r.GetDateTime(4)).Returns(DateTime.Now);
            mockReader.Setup(r => r.GetString(5)).Returns("Test Artist");
            mockReader.Setup(r => r.GetString(6)).Returns("Test Type");
            mockReader.Setup(r => r.GetString(7)).Returns("Test Description");
            mockReader.Setup(r => r.GetValue(8)).Returns(new string[] { "Track1", "Track2" });
            mockReader.Setup(r => r.GetValue(2)).Returns(new byte[] { 1, 2, 3 });

            _mockDataSource.Setup(ds => ds.ExecuteReader(It.IsAny<string>())).Returns(mockReader.Object);

            // Act
            var album = _repository.GetSingleAlbum(albumId);

            // Assert
            album.Should().NotBeNull();
            album.Id.Should().Be(albumId);
        }
    }
}