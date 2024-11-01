using Moq;
using DataAccess;

namespace RazorMusic.Tests
{
    public class AuthControllerTest
    {
        private readonly Mock<UserRepository> _userRepositoryMock;
        private readonly AuthController _authController;

        public AuthControllerTest()
        {
            _userRepositoryMock = new Mock<UserRepository>();
            _authController = new AuthController
            {
                userRepository = _userRepositoryMock.Object
            };
            _authController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public async void Login_ValidCredentials_ReturnsOk()
        {
            // Arrange
            string email = "nbrau23@student.sdu.dk";
            string password = "N1ck1sC00l";
            _userRepositoryMock.Setup(repo => repo.VerifyUserCredentials(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var result = _authController.Login(email, password) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True(((dynamic)result.Value).success);
        }

        [Fact]
        public async void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            string email = "test@example.com";
            string password = "wrongpassword";
            _userRepositoryMock.Setup(repo => repo.VerifyUserCredentials(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = _authController.Login(email, password) as UnauthorizedObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            Assert.False(((dynamic)result.Value).success);
        }

        [Fact]
        public async void Logout_ReturnsOk()
        {
            // Act
            var result = _authController.Logout() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True(((dynamic)result.Value).success);
        }
    }
}