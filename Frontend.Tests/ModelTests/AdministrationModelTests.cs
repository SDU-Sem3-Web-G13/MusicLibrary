using Backend.Services.ServiceDtos;

namespace Frontend.Tests.ModelTests
{
    public class AdministrationModelTests
    {
        private readonly Mock<IAdministrationService> _mockAdministrationService;
        private readonly AdministrationModel _administrationModel;

        public AdministrationModelTests()
        {
            _mockAdministrationService = new Mock<IAdministrationService>();
            _administrationModel = new AdministrationModel(_mockAdministrationService.Object);
        }

        [Fact]
        public void GetUsers_ReturnsListOfUsers()
        {
            // Arrange
            var expectedUsers = new List<IUserServiceDto>
            {
                new UserServiceDto(1, "User1", "user1@example.com", true),
                new UserServiceDto(2, "User2", "user2@example.com", true)
            };
            _mockAdministrationService.Setup(service => service.GetUsers()).Returns(expectedUsers);

            // Act
            var result = _administrationModel.GetUsers().Select(user => new UserServiceDto(user.Id, user.Name, user.Mail, user.IsAdmin)).ToList();

            // Assert
            result.Should().BeEquivalentTo(expectedUsers);
        }

        [Fact]
        public void DeleteUser_CallsServiceDeleteUser()
        {
            // Arrange
            var userId = 1;
            _mockAdministrationService.Setup(service => service.DeleteUser(userId));

            // Act
            _administrationModel.DeleteUser(userId);

            // Assert
            _mockAdministrationService.Verify(service => service.DeleteUser(userId), Times.Once);
        }
    }
}