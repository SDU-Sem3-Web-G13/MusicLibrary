using BCrypt.Net;

namespace Frontend.Tests.ModelTests
{
    public class LoginRegisterModelTests
    {
        private readonly Mock<ILoginRegisterService> _mockLoginRegisterService;
        private readonly LoginRegisterModel _loginRegisterModel;

        public LoginRegisterModelTests()
        {
            _mockLoginRegisterService = new Mock<ILoginRegisterService>();
            _loginRegisterModel = new LoginRegisterModel(_mockLoginRegisterService.Object);
        }

        [Fact]
        public void EmailExists_ReturnsTrue_WhenEmailExists()
        {
            // Arrange
            var email = "test@example.com";
            _mockLoginRegisterService.Setup(service => service.EmailExists(email)).Returns(true);

            // Act
            var result = _loginRegisterModel.EmailExists(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetFixedSalt_ReturnsSalt()
        {
            // Arrange
            var expectedSalt = BCrypt.Net.BCrypt.GenerateSalt();
            _mockLoginRegisterService.Setup(service => service.GetFixedSalt()).Returns(expectedSalt);

            // Act
            var result = _loginRegisterModel.GetFixedSalt();

            // Assert
            Assert.Equal(expectedSalt, result);
        }

        [Fact]
        public void ConvertToHex_ReturnsHexString()
        {
            // Arrange
            var input = "test";
            var expectedHex = "74657374";

            // Act
            var result = _loginRegisterModel.ConvertToHex(input);

            // Assert
            Assert.Equal(expectedHex, result);
        }

        [Fact]
        public void ValidateCredentials_ReturnsTrue_WhenCredentialsAreValid()
        {
            // Arrange
            var emailHash = "emailHash";
            var passwordHash = "passwordHash";
            _mockLoginRegisterService.Setup(service => service.ValidateCredentials(emailHash, passwordHash)).Returns(true);

            // Act
            var result = _loginRegisterModel.ValidateCredentials(emailHash, passwordHash);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetUserId_ReturnsUserId()
        {
            // Arrange
            var email = "test@example.com";
            var expectedUserId = 1;
            _mockLoginRegisterService.Setup(service => service.GetUserId(email)).Returns(expectedUserId);

            // Act
            var result = _loginRegisterModel.GetUserId(email);

            // Assert
            Assert.Equal(expectedUserId, result);
        }

        [Fact]
        public void IsAdmin_ReturnsTrue_WhenUserIsAdmin()
        {
            // Arrange
            var userId = 1;
            _mockLoginRegisterService.Setup(service => service.IsAdmin(userId)).Returns(true);

            // Act
            var result = _loginRegisterModel.IsAdmin(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AddUser_CallsServiceAddUser()
        {
            // Arrange
            var firstName = "John";
            var email = "john@example.com";

            // Act
            _loginRegisterModel.AddUser(firstName, email);

            // Assert
            _mockLoginRegisterService.Verify(service => service.AddUser(firstName, email), Times.Once);
        }

        [Fact]
        public void AddUserCredentials_CallsServiceAddUserCredentials()
        {
            // Arrange
            var emailHash = new byte[] { 1, 2, 3 };
            var passwordHash = new byte[] { 4, 5, 6 };

            // Act
            _loginRegisterModel.AddUserCredentials(emailHash, passwordHash);

            // Assert
            _mockLoginRegisterService.Verify(service => service.AddUserCredentials(emailHash, passwordHash), Times.Once);
        }
    }
}