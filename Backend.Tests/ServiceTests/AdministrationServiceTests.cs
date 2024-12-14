using Backend.DataAccess.Dtos;
using Backend.Services.ServiceDtos;

namespace Backend.Tests.ServiceTests
{
    public class AdministrationServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ILoginRegisterService> _loginRegisterServiceMock;
        private readonly AdministrationService _administrationService;

        public AdministrationServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _loginRegisterServiceMock = new Mock<ILoginRegisterService>();
            _administrationService = new AdministrationService(_userRepositoryMock.Object, _loginRegisterServiceMock.Object);
        }

        [Fact]
        public void GetUsers_ReturnsListOfUsers()
        {
            // Arrange
            var users = new List<IUserDto> { new UserDto(1, "test", "test@example.com", true) };
            _userRepositoryMock.Setup(repo => repo.GetUsers()).Returns(users);

            // Act
            var result = _administrationService.GetUsers().Select(user => new UserDto(user.Id, user.Name, user.Mail, user.IsAdmin)).ToList();

            // Assert
            result.Should().BeEquivalentTo(users);
        }

        [Fact]
        public void DeleteUser_DeletesUserAndCredentials()
        {
            // Arrange
            int userId = 1;
            var user = new UserDto(userId, "test", "test@example.com", true);
            var fixedSalt = BCrypt.Net.BCrypt.GenerateSalt();
            var emailHash = BCrypt.Net.BCrypt.HashPassword(user.Mail, fixedSalt);
            var emailHashBytes = System.Text.Encoding.UTF8.GetBytes(emailHash);

            _userRepositoryMock.Setup(repo => repo.GetSingleUser(userId)).Returns(user);
            _loginRegisterServiceMock.Setup(service => service.GetFixedSalt()).Returns(fixedSalt);

            // Act
            _administrationService.DeleteUser(userId);

            // Assert
            _userRepositoryMock.Verify(repo => repo.DeleteUserCredentials(emailHashBytes), Times.Once);
            _userRepositoryMock.Verify(repo => repo.DeleteUser(userId), Times.Once);
        }
    }
}