using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Frontend.Tests.ControllerTests
{
    public class AdminViewTests
    {
        private readonly Mock<ILoginRegisterModel> _mockLoginRegisterModel;
        private readonly Mock<IAlbumsModel> _mockAlbumsModel;
        private readonly Mock<IAdministrationModel> _mockAdministrationModel;
        private readonly AdminViewModel _adminViewModel;
        private readonly DefaultHttpContext _httpContext;

        public AdminViewTests()
        {
            _mockLoginRegisterModel = new Mock<ILoginRegisterModel>();
            _mockAlbumsModel = new Mock<IAlbumsModel>();
            _mockAdministrationModel = new Mock<IAdministrationModel>();
            _adminViewModel = new AdminViewModel(_mockLoginRegisterModel.Object, _mockAlbumsModel.Object, _mockAdministrationModel.Object);

            var services = new ServiceCollection();
            services.AddDistributedMemoryCache();
            services.AddSession();
            var serviceProvider = services.BuildServiceProvider();

            _httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };
            _httpContext.Features.Set<ISessionFeature>(new SessionFeature { Session = new MockSession() });
            _adminViewModel.PageContext.HttpContext = _httpContext;
        }

        [Fact]
        public void OnGet_ValidSession_CallsGetUsersAndAlbums()
        {
            // Arrange
            _httpContext.Session.SetInt32("IsLoggedIn", 1);
            _httpContext.Session.SetInt32("userId", 1);
            var userList = new List<UserModel>
            {
                new UserModel(1, "Test User", "testuser@example.com", false)
            };
            _mockAdministrationModel.Setup(m => m.GetUsers()).Returns(userList);
            var albumList = new List<AlbumModel>
            {
                new AlbumModel("Test Album", DateTime.Now, "Test Artist", "Single", "Test Description", new [] { "Test Track" })
            };
            _mockAlbumsModel.Setup(m => m.GetAlbums(It.IsAny<int>())).Returns(albumList);


            // Act
            _adminViewModel.OnGet();

            // Assert
            _mockAdministrationModel.Verify(m => m.GetUsers(), Times.Once);
            _mockAlbumsModel.Verify(m => m.GetAlbums(It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Fact]
        public void OnGetDeleteAlbum_DeletesAlbum_ReturnsSuccess()
        {
            // Arrange
            var albumId = 1;
            _mockAlbumsModel.Setup(m => m.DeleteAlbum(albumId));
            var userList = new List<UserModel>
            {
                new UserModel(1, "Test User", "testuser@example.com", false)
            };
            _mockAdministrationModel.Setup(m => m.GetUsers()).Returns(userList);

            var albumList = new List<AlbumModel>
            {
                new AlbumModel("Test Album", DateTime.Now, "Test Artist", "Single", "Test Description", new [] { "Test Track" })
            };
            _mockAlbumsModel.Setup(m => m.GetAlbums(It.IsAny<int>())).Returns(albumList);

            // Act
            var result = _adminViewModel.OnGetDeleteAlbum(albumId) as JsonResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            var successProperty = result.Value.GetType().GetProperty("success");
            Assert.NotNull(successProperty);
            var successValue = successProperty.GetValue(result.Value, null);
            Assert.NotNull(successValue);
            Assert.True((bool)successValue);
            _mockAlbumsModel.Verify(m => m.DeleteAlbum(albumId), Times.Once);
        }

        [Fact]
        public void OnGetDeleteUser_DeletesUser_ReturnsSuccess()
        {
            // Arrange
            var userId = 2;
            _mockLoginRegisterModel.Setup(m => m.IsAdmin(userId)).Returns(false);
            _mockAdministrationModel.Setup(m => m.DeleteUser(userId));

            var userList = new List<UserModel>
            {
                new UserModel(1, "Test User", "testuser@example.com", false)
            };
            _mockAdministrationModel.Setup(m => m.GetUsers()).Returns(userList);

            var albumList = new List<AlbumModel>
            {
                new AlbumModel("Test Album", DateTime.Now, "Test Artist", "Single", "Test Description", new [] { "Test Track" })
            };
            _mockAlbumsModel.Setup(m => m.GetAlbums(It.IsAny<int>())).Returns(albumList);

            // Act
            var result = _adminViewModel.OnGetDeleteUser(userId) as JsonResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            var successProperty = result.Value.GetType().GetProperty("success");
            Assert.NotNull(successProperty);
            var successValue = successProperty.GetValue(result.Value, null);
            Assert.NotNull(successValue);
            Assert.True((bool)successValue);
            _mockAlbumsModel.Verify(m => m.DeleteAllUserAlbums(userId), Times.Once);
            _mockAdministrationModel.Verify(m => m.DeleteUser(userId), Times.Once);
        }

        [Fact]
        public void OnGetDeleteUser_CannotDeleteAdminUser_ReturnsFailure()
        {
            // Arrange
            var userId = 1;
            _mockLoginRegisterModel.Setup(m => m.IsAdmin(userId)).Returns(true);
            var userList = new List<UserModel>
            {
                new UserModel(1, "Test User", "testuser@example.com", true)
            };
            _mockAdministrationModel.Setup(m => m.GetUsers()).Returns(userList);


            // Act
            var result = _adminViewModel.OnGetDeleteUser(userId) as JsonResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            var successProperty = result.Value.GetType().GetProperty("success");
            Assert.NotNull(successProperty);
            var successValue = successProperty.GetValue(result.Value, null);
            Assert.NotNull(successValue);
            Assert.False((bool)successValue);
            Assert.NotNull(result.Value);
            var messageProperty = result.Value.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            Assert.Equal("Cannot delete admin user", messageProperty.GetValue(result.Value, null));
        }

        [Fact]
        public void ValidateSessionStorage_NotLoggedIn_RedirectsToLogin()
        {
            // Arrange
            _httpContext.Session.SetInt32("IsLoggedIn", 0);

            var userList = new List<UserModel>
            {
                new UserModel(1, "Test User", "testuser@example.com", false)
            };
            _mockAdministrationModel.Setup(m => m.GetUsers()).Returns(userList);

            var albumList = new List<AlbumModel>
            {
                new AlbumModel("Test Album", DateTime.Now, "Test Artist", "Single", "Test Description", new [] { "Test Track" })
            };
            _mockAlbumsModel.Setup(m => m.GetAlbums(It.IsAny<int>())).Returns(albumList);

            // Act
            _adminViewModel.OnGet();

            // Assert
            Assert.Equal("/Login", _httpContext.Response.Headers["Location"]);
        }

        [Fact]
        public void ValidateSessionStorage_NotAdmin_RedirectsToIndex()
        {
            // Arrange
            _httpContext.Session.SetInt32("IsLoggedIn", 1);
            _httpContext.Session.SetInt32("userId", 2);

            var userList = new List<UserModel>
            {
                new UserModel(2, "Test User", "testuser@example.com", false)
            };
            _mockAdministrationModel.Setup(m => m.GetUsers()).Returns(userList);

            var albumList = new List<AlbumModel>
            {
                new AlbumModel("Test Album", DateTime.Now, "Test Artist", "Single", "Test Description", new [] { "Test Track" })
            };
            _mockAlbumsModel.Setup(m => m.GetAlbums(It.IsAny<int>())).Returns(albumList);

            // Act
            _adminViewModel.OnGet();

            // Assert
            Assert.Equal("/Index", _httpContext.Response.Headers["Location"]);
        }
    }
}