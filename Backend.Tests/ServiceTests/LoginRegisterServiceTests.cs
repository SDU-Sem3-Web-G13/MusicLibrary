using BCrypt.Net;

namespace Backend.Tests.ServiceTests
{
    public class LoginRegisterServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly LoginRegisterService _loginRegisterService;

        public LoginRegisterServiceTests()
        {
            Environment.SetEnvironmentVariable("BCRYPT_SALT", Convert.ToBase64String(Encoding.UTF8.GetBytes(BCrypt.Net.BCrypt.GenerateSalt())));
            _userRepositoryMock = new Mock<IUserRepository>();
            _loginRegisterService = new LoginRegisterService(_userRepositoryMock.Object);
        }

        [Fact]
        public void EmailExists_ReturnsTrue_WhenEmailExists()
        {
            // Arrange
            string email = "test@example.com";
            _userRepositoryMock.Setup(repo => repo.EmailExists(email)).Returns(true);

            // Act
            var result = _loginRegisterService.EmailExists(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EmailExists_ReturnsFalse_WhenEmailDoesNotExist()
        {
            // Arrange
            string email = "test@example.com";
            _userRepositoryMock.Setup(repo => repo.EmailExists(email)).Returns(false);

            // Act
            var result = _loginRegisterService.EmailExists(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateCredentials_ReturnsTrue_WhenCredentialsAreValid()
        {
            // Arrange
            string emailHash = "emailHash";
            string passwordHash = "passwordHash";
            _userRepositoryMock.Setup(repo => repo.GetHashedPassword(emailHash)).Returns(passwordHash);

            // Act
            var result = _loginRegisterService.ValidateCredentials(emailHash, passwordHash);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateCredentials_ReturnsFalse_WhenCredentialsAreInvalid()
        {
            // Arrange
            string emailHash = "emailHash";
            string passwordHash = "passwordHash";
            _userRepositoryMock.Setup(repo => repo.GetHashedPassword(emailHash)).Returns("differentPasswordHash");

            // Act
            var result = _loginRegisterService.ValidateCredentials(emailHash, passwordHash);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetFixedSalt_ReturnsFixedSalt()
        {
            // Act
            var result = _loginRegisterService.GetFixedSalt();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void AddUserCredentials_CallsInsertUserCredentials()
        {
            // Arrange
            byte[] emailHash = Encoding.UTF8.GetBytes("emailHash");
            byte[] passwordHash = Encoding.UTF8.GetBytes("passwordHash");

            // Act
            _loginRegisterService.AddUserCredentials(emailHash, passwordHash);

            // Assert
            _userRepositoryMock.Verify(repo => repo.InsertUserCredentials(emailHash, passwordHash), Times.Once);
        }

        [Fact]
        public void GetUserId_ReturnsUserId()
        {
            // Arrange
            string email = "test@example.com";
            int userId = 1;
            _userRepositoryMock.Setup(repo => repo.GetUserId(email)).Returns(userId);

            // Act
            var result = _loginRegisterService.GetUserId(email);

            // Assert
            Assert.Equal(userId, result);
        }

        [Fact]
        public void IsAdmin_ReturnsTrue_WhenUserIsAdmin()
        {
            // Arrange
            int userId = 1;
            _userRepositoryMock.Setup(repo => repo.IsAdmin(userId)).Returns(true);

            // Act
            var result = _loginRegisterService.IsAdmin(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAdmin_ReturnsFalse_WhenUserIsNotAdmin()
        {
            // Arrange
            int userId = 1;
            _userRepositoryMock.Setup(repo => repo.IsAdmin(userId)).Returns(false);

            // Act
            var result = _loginRegisterService.IsAdmin(userId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddUser_CallsAddUser()
        {
            // Arrange
            string firstName = "Test";
            string email = "test@example.com";

            // Act
            _loginRegisterService.AddUser(firstName, email);

            // Assert
            _userRepositoryMock.Verify(repo => repo.AddUser(firstName, email), Times.Once);
        }
    }
}