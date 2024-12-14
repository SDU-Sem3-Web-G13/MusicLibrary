namespace Frontend.Tests.ControllerTests
{
    public class LoginViewTestsTest
    {
        private readonly Mock<ILoginRegisterModel> _mockLoginRegisterModel;
        private readonly LoginViewModel _loginViewModel;
        private readonly DefaultHttpContext _httpContext;

        public LoginViewTestsTest()
        {
            _mockLoginRegisterModel = new Mock<ILoginRegisterModel>();
            _mockLoginRegisterModel.Setup(model => model.GetFixedSalt()).Returns(BCrypt.Net.BCrypt.GenerateSalt());
            _loginViewModel = new LoginViewModel(_mockLoginRegisterModel.Object);
            
            var services = new ServiceCollection();
            services.AddDistributedMemoryCache();
            services.AddSession();
            var serviceProvider = services.BuildServiceProvider();

            _httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };
            _httpContext.Features.Set<ISessionFeature>(new SessionFeature { Session = new MockSession() });
            _loginViewModel.PageContext.HttpContext = _httpContext;
        }

        [Fact]
        public void OnPost_EmailDoesNotExist_ReturnsPage()
        {
            // Arrange
            _loginViewModel.LoginUser.Email = "test@example.com";
            _mockLoginRegisterModel.Setup(model => model.EmailExists(It.IsAny<string>())).Returns(false);

            // Act
            var result = _loginViewModel.OnPost();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public void OnPost_InvalidCredentials_ReturnsPageWithErrorMessage()
        {
            // Arrange
            _loginViewModel.LoginUser.Email = "test@example.com";
            _loginViewModel.LoginUser.Password = "password";
            _mockLoginRegisterModel.Setup(model => model.EmailExists(It.IsAny<string>())).Returns(true);
            _mockLoginRegisterModel.Setup(model => model.ValidateCredentials(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = _loginViewModel.OnPost();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal("Invalid password.", _loginViewModel.ErrorMessage);
        }

        [Fact]
        public void OnPost_ValidCredentials_RedirectsToAlbumsView()
        {
            // Arrange
            _loginViewModel.LoginUser.Email = "test@example.com";
            _loginViewModel.LoginUser.Password = "password";
            var userId = 1;
            _mockLoginRegisterModel.Setup(model => model.EmailExists(It.IsAny<string>())).Returns(true);
            _mockLoginRegisterModel.Setup(model => model.ValidateCredentials(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _mockLoginRegisterModel.Setup(model => model.GetUserId(It.IsAny<string>())).Returns(userId);
            _mockLoginRegisterModel.Setup(model => model.IsAdmin(It.IsAny<int>())).Returns(false);

            // Act
            var result = _loginViewModel.OnPost();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            var redirectToPageResult = result as RedirectToPageResult;
            Assert.NotNull(redirectToPageResult);
            Assert.NotNull(redirectToPageResult);
            Assert.Equal("AlbumsView", redirectToPageResult?.PageName);
            Assert.Equal(1, _httpContext.Session.GetInt32("IsLoggedIn"));
            Assert.Equal(userId, _httpContext.Session.GetInt32("userId"));
            Assert.Equal(0, _httpContext.Session.GetInt32("IsAdmin"));
        }

        [Fact]
        public void OnPost_ValidCredentials_AdminUser_RedirectsToAlbumsView()
        {
            // Arrange
            _loginViewModel.LoginUser.Email = "admin@example.com";
            _loginViewModel.LoginUser.Password = "adminpassword";
            var userId = 1;
            _mockLoginRegisterModel.Setup(model => model.EmailExists(It.IsAny<string>())).Returns(true);
            _mockLoginRegisterModel.Setup(model => model.ValidateCredentials(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _mockLoginRegisterModel.Setup(model => model.GetUserId(It.IsAny<string>())).Returns(userId);
            _mockLoginRegisterModel.Setup(model => model.IsAdmin(It.IsAny<int>())).Returns(true);

            // Act
            var result = _loginViewModel.OnPost();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            var redirectToPageResult = result as RedirectToPageResult;
            Assert.NotNull(redirectToPageResult);
            Assert.Equal("AlbumsView", redirectToPageResult?.PageName);
            Assert.Equal(1, _httpContext.Session.GetInt32("IsLoggedIn"));
            Assert.Equal(userId, _httpContext.Session.GetInt32("userId"));
            Assert.Equal(1, _httpContext.Session.GetInt32("IsAdmin"));
        }
    }
}