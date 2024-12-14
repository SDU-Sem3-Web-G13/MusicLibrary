namespace Frontend.Tests.ControllerTests
{
    public class RegisterViewTestsTest
    {
        private readonly Mock<ILoginRegisterModel> _mockLoginRegisterModel;
        private readonly RegisterViewModel _registerViewModel;
        private readonly DefaultHttpContext _httpContext;

        public RegisterViewTestsTest()
        {
            _mockLoginRegisterModel = new Mock<ILoginRegisterModel>();
            _mockLoginRegisterModel.Setup(model => model.GetFixedSalt()).Returns(BCrypt.Net.BCrypt.GenerateSalt());
            _registerViewModel = new RegisterViewModel(_mockLoginRegisterModel.Object);
            
            var services = new ServiceCollection();
            services.AddDistributedMemoryCache();
            services.AddSession();
            var serviceProvider = services.BuildServiceProvider();

            _httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };
            _httpContext.Features.Set<ISessionFeature>(new SessionFeature { Session = new MockSession() });
            _registerViewModel.PageContext.HttpContext = _httpContext;
        }

        [Fact]
        public void OnPost_EmailAlreadyExists_ReturnsPageWithErrorMessage()
        {
            // Arrange
            _registerViewModel.LoginUser.Email = "test@example.com";
            _registerViewModel.LoginUser.Password = "password";
            _mockLoginRegisterModel.Setup(model => model.EmailExists(It.IsAny<string>())).Returns(true);

            // Act
            var result = _registerViewModel.OnPost();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal("User already exists", _registerViewModel.ErrorMessage);
        }

        [Fact]
        public void OnPost_RegistrationSucceeds_RedirectsToLoginView()
        {
            // Arrange
            _registerViewModel.LoginUser.Email = "test@example.com";
            _registerViewModel.LoginUser.Password = "password";
            _mockLoginRegisterModel.Setup(model => model.EmailExists(It.IsAny<string>())).Returns(false);
            // Act
            var result = _registerViewModel.OnPost();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}