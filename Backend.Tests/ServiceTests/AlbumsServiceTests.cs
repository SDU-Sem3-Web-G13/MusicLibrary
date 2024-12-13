namespace Backend.Tests.ServiceTests
{
    public class AlbumsServiceTests
    {
        private readonly Mock<IAlbumRepository> _albumRepositoryMock;
        private readonly AlbumsService _albumsService;

        public AlbumsServiceTests()
        {
            _albumRepositoryMock = new Mock<IAlbumRepository>();
            _albumsService = new AlbumsService(_albumRepositoryMock.Object);
        }

        [Fact]
        public void GetAlbums_ReturnsListOfAlbums()
        {
            // Arrange
            int userId = 1;
            var albums = new List<AlbumModel> { new AlbumModel("Test Album", DateTime.Now, "Test Artist", "Test Type", "Test Description", new string[] { "Track1", "Track2" }) { Id = 1, OwnerId = userId } };
            _albumRepositoryMock.Setup(repo => repo.GetAlbums(userId)).Returns(albums);

            // Act
            var result = _albumsService.GetAlbums(userId);

            // Assert
            Assert.Equal(albums, result);
        }

        [Fact]
        public void GetSingleAlbum_ReturnsAlbum()
        {
            // Arrange
            int albumId = 1;
            var album = new AlbumModel("Test Album", DateTime.Now, "Test Artist", "Test Type", "Test Description", new string[] { "Track1", "Track2" }) { Id = albumId };
            _albumRepositoryMock.Setup(repo => repo.GetSingleAlbum(albumId)).Returns(album);

            // Act
            var result = _albumsService.GetSingleAlbum(albumId);

            // Assert
            Assert.Equal(album, result);
        }

        [Fact]
        public void DeleteAlbum_DeletesAlbum()
        {
            // Arrange
            int albumId = 1;

            // Act
            _albumsService.DeleteAlbum(albumId);

            // Assert
            _albumRepositoryMock.Verify(repo => repo.DeleteAlbum(albumId), Times.Once);
        }

        [Fact]
        public void DeleteAllUserAlbums_DeletesAllAlbumsForUser()
        {
            // Arrange
            int userId = 1;

            // Act
            _albumsService.DeleteAllUserAlbums(userId);

            // Assert
            _albumRepositoryMock.Verify(repo => repo.DeleteAllUserAlbums(userId), Times.Once);
        }

        [Fact]
        public void AddAlbum_AddsAlbum()
        {
            // Arrange
            int owner = 1;
            byte[] cover = new byte[] { 0x20, 0x20 };
            string albumName = "Test Album";
            DateTime releaseDate = DateTime.Now;
            string artist = "Test Artist";
            string type = "Test Type";
            string description = "Test Description";
            string[] tracks = new string[] { "Track1", "Track2" };

            // Act
            _albumsService.AddAlbum(owner, cover, albumName, releaseDate, artist, type, description, tracks);

            // Assert
            _albumRepositoryMock.Verify(repo => repo.AddAlbum(owner, cover, albumName, releaseDate, artist, type, description, tracks), Times.Once);
        }

        [Fact]
        public void ModifyAlbum_ModifiesAlbum()
        {
            // Arrange
            int id = 1;
            int owner = 1;
            byte[] cover = new byte[] { 0x20, 0x20 };
            string albumName = "Test Album";
            DateTime releaseDate = DateTime.Now;
            string artist = "Test Artist";
            string type = "Test Type";
            string description = "Test Description";
            string[] tracks = new string[] { "Track1", "Track2" };

            // Act
            _albumsService.ModifyAlbum(id, owner, cover, albumName, releaseDate, artist, type, description, tracks);

            // Assert
            _albumRepositoryMock.Verify(repo => repo.ModifyAlbum(id, owner, cover, albumName, releaseDate, artist, type, description, tracks), Times.Once);
        }
    }
}