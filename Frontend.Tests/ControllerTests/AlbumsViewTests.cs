namespace Frontend.Tests.ControllerTests
{
    public class AlbumsViewTests
    {
        private readonly Mock<IAlbumsModel> _mockAlbumsModel;
        private readonly DefaultHttpContext _httpContext;
        private readonly AlbumsViewModel _albumsViewModel;

        public AlbumsViewTests()
        {
            _mockAlbumsModel = new Mock<IAlbumsModel>();
            _albumsViewModel = new AlbumsViewModel(_mockAlbumsModel.Object);

            var services = new ServiceCollection();
            services.AddDistributedMemoryCache();
            services.AddSession();
            var serviceProvider = services.BuildServiceProvider();

            _httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };
            _httpContext.Features.Set<ISessionFeature>(new SessionFeature { Session = new MockSession() });
            _albumsViewModel.PageContext.HttpContext = _httpContext;
        }

        [Fact]
        public void ValidateSessionStorage_NotLoggedIn_RedirectsToLogin()
        {
            // Arrange
            _httpContext.Session.SetInt32("IsLoggedIn", 0);
            _httpContext.Session.SetInt32("userId", 1);

            var albumList = new List<AlbumModel>
            {
                new AlbumModel("Test Album", DateTime.Now, "Test Artist", "Single", "Test Description", new [] { "Test Track" })
            };
            _mockAlbumsModel.Setup(m => m.GetAlbums(It.IsAny<int>())).Returns(albumList);

            // Act
            _albumsViewModel.OnGet();

            // Assert
            Assert.Equal("/Login", _httpContext.Response.Headers["Location"]);
        }

        [Fact]
        public void OnGetGetAlbum_ReturnsJsonResult()
        {
            // Arrange
            var album = new AlbumModel("Test Album", DateTime.Now, "Test Artist", "Single", "Test Description", new [] { "Test Track" });
            _mockAlbumsModel.Setup(m => m.GetSingleAlbum(It.IsAny<int>())).Returns(album);

            // Act
            var result = _albumsViewModel.OnGetGetAlbum(1);

            // Assert
            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public void OnGetDeleteAlbum_ReturnsJsonResult()
        {
            // Arrange
            var albumList = new List<AlbumModel>
            {
                new AlbumModel("Test Album", DateTime.Now, "Test Artist", "Single", "Test Description", new [] { "Test Track" })
            };
            _mockAlbumsModel.Setup(m => m.GetAlbums(It.IsAny<int>())).Returns(albumList);

            // Act
            var result = _albumsViewModel.OnGetDeleteAlbum(1);

            // Assert
            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public void OnGetClearInputAlbum_ReturnsJsonResult()
        {
            // Act
            var result = _albumsViewModel.OnGetClearInputAlbum();

            // Assert
            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public void OnPostAddAlbum_ReturnsJsonResult()
        {
            // Arrange
            var albumList = new List<AlbumModel>
            {
                new AlbumModel("Test Album", DateTime.Now, "Test Artist", "Single", "Test Description", new [] { "Test Track" })
            };
            _mockAlbumsModel.Setup(m => m.GetAlbums(It.IsAny<int>())).Returns(albumList);

            // Act
            var result = _albumsViewModel.OnPostAddAlbum();

            // Assert
            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public void OnPostEditAlbum_ReturnsJsonResult()
        {
            // Arrange
            var albumList = new List<AlbumModel>
            {
                new AlbumModel("Test Album", DateTime.Now, "Test Artist", "Single", "Test Description", new [] { "Test Track" })
            };
            _mockAlbumsModel.Setup(m => m.GetAlbums(It.IsAny<int>())).Returns(albumList);

            // Act
            var result = _albumsViewModel.OnPostEditAlbum();

            // Assert
            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public void OnGetRetrieveAlbumCover_ReturnsJsonResult()
        {
            // Arrange
            var album = new AlbumModel("Test Album", DateTime.Now, "Test Artist", "Single", "Test Description", new [] { "Test Track" });
            _mockAlbumsModel.Setup(m => m.GetSingleAlbum(It.IsAny<int>())).Returns(album);

            // Act
            var result = _albumsViewModel.OnGetRetrieveAlbumCover(1);

            // Assert
            Assert.IsType<JsonResult>(result);
        }
    }
}