using Backend.Services.ServiceDtos;
using Microsoft.AspNetCore.Http;

namespace Frontend.Tests.ModelTests
{
    public class AlbumsModelTest
    {
        private readonly Mock<IAlbumsService> _mockAlbumsService;
        private readonly AlbumsModel _albumsModel;

        public AlbumsModelTest()
        {
            _mockAlbumsService = new Mock<IAlbumsService>();
            _albumsModel = new AlbumsModel(_mockAlbumsService.Object);
        }

        [Fact]
        public void GetAlbums_ReturnsListOfAlbums()
        {
            // Arrange
            var userId = 1;
            var expectedAlbums = new List<IAlbumServiceDto>
            {
                new AlbumServiceDto("Album1", DateTime.Now, "Artist1", "Type1", "Description1", new string[] { "Track1", "Track2" }) { Id = 1 },
                new AlbumServiceDto("Album2", DateTime.Now, "Artist2", "Type2", "Description2", new string[] { "Track3", "Track4" }) { Id = 2 }
            };
            _mockAlbumsService.Setup(service => service.GetAlbums(userId)).Returns(expectedAlbums);

            // Act
            var result = _albumsModel.GetAlbums(userId).Select(album => new AlbumModel(album.AlbumName, album.ReleaseDate, album.Artist, album.AlbumType, album.Description, album.Tracks) { Id = album.Id }).ToList();

            // Assert
            result.Should().BeEquivalentTo(expectedAlbums);
        }

        [Fact]
        public void GetSingleAlbum_ReturnsAlbum()
        {
            // Arrange
            var albumId = 1;
            var expectedAlbum = new AlbumServiceDto("Album1", DateTime.Now, "Artist1", "Type1", "Description1", new string[] { "Track1", "Track2" }) { Id = albumId };
            _mockAlbumsService.Setup(service => service.GetSingleAlbum(albumId)).Returns(expectedAlbum);

            // Act
            var result = _albumsModel.GetSingleAlbum(albumId);

            // Assert
            result.Should().BeEquivalentTo(expectedAlbum);
        }

        [Fact]
        public void DeleteAlbum_CallsServiceDeleteAlbum()
        {
            // Arrange
            var albumId = 1;
            _mockAlbumsService.Setup(service => service.DeleteAlbum(albumId));

            // Act
            _albumsModel.DeleteAlbum(albumId);

            // Assert
            _mockAlbumsService.Verify(service => service.DeleteAlbum(albumId), Times.Once);
        }

        [Fact]
        public void DeleteAllUserAlbums_CallsServiceDeleteAllUserAlbums()
        {
            // Arrange
            var userId = 1;
            _mockAlbumsService.Setup(service => service.DeleteAllUserAlbums(userId));

            // Act
            _albumsModel.DeleteAllUserAlbums(userId);

            // Assert
            _mockAlbumsService.Verify(service => service.DeleteAllUserAlbums(userId), Times.Once);
        }

        [Fact]
        public void ModifyAlbum_CallsServiceModifyAlbum()
        {
            // Arrange
            var albumId = 1;
            var owner = 1;
            var albumName = "Album1";
            var releaseDate = DateTime.Now;
            var artist = "Artist1";
            var type = "Type1";
            var description = "Description1";
            var tracks = new string[] { "Track1", "Track2" };
            var cover = new Mock<IFormFile>();
            var coverImageBytes = new byte[] { 1, 2, 3 };
            var memoryStream = new MemoryStream(coverImageBytes);

            cover.Setup(c => c.CopyTo(It.IsAny<Stream>())).Callback<Stream>(s => memoryStream.CopyTo(s));

            _mockAlbumsService.Setup(service => service.ModifyAlbum(albumId, owner, coverImageBytes, albumName, releaseDate, artist, type, description, tracks));

            // Act
            _albumsModel.ModifyAlbum(albumId, owner, cover.Object, albumName, releaseDate, artist, type, description, tracks);

            // Assert
            _mockAlbumsService.Verify(service => service.ModifyAlbum(albumId, owner, coverImageBytes, albumName, releaseDate, artist, type, description, tracks), Times.Once);
        }

        [Fact]
        public void AddAlbum_CallsServiceAddAlbum()
        {
            // Arrange
            var owner = 1;
            var albumName = "Album1";
            var releaseDate = DateTime.Now;
            var artist = "Artist1";
            var type = "Type1";
            var description = "Description1";
            var tracks = new string[] { "Track1", "Track2" };
            var cover = new Mock<IFormFile>();
            var coverImageBytes = new byte[] { 1, 2, 3 };
            var memoryStream = new MemoryStream(coverImageBytes);
            
            cover.Setup(c => c.CopyTo(It.IsAny<Stream>())).Callback<Stream>(s => memoryStream.CopyTo(s));

            _mockAlbumsService.Setup(service => service.AddAlbum(owner, coverImageBytes, albumName, releaseDate, artist, type, description, tracks));

            // Act
            _albumsModel.AddAlbum(owner, cover.Object, albumName, releaseDate, artist, type, description, tracks);

            // Assert
            _mockAlbumsService.Verify(service => service.AddAlbum(owner, coverImageBytes, albumName, releaseDate, artist, type, description, tracks), Times.Once);
        }
    }
}