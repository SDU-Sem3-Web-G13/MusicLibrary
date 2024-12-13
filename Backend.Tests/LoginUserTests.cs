namespace Backend.Tests
{
    public class LoginUserTests
    {
        private Mock<IUserRepository> mockUserRepository;

        public LoginUserTests()
        {
            mockUserRepository = new Mock<IUserRepository>();
        }

        [Fact]
        public void ValidLoginUser_ShouldPassValidation()
        {
            var user = new LoginUser
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123",
                SourcePage = "RegistrationPage"
            };

            var validationResults = ValidateModel(user, mockUserRepository.Object);

            Assert.Empty(validationResults);
        }

        [Fact]
        public void MissingFirstName_ShouldFailValidation()
        {
            var user = new LoginUser
            {
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123",
                SourcePage = "RegistrationPage"
            };

            var validationResults = ValidateModel(user, mockUserRepository.Object);

            Assert.Contains(validationResults, v => v.ErrorMessage == "First name is required.");
        }

        [Fact]
        public void InvalidEmailFormat_ShouldFailValidation()
        {
            var user = new LoginUser
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "invalid-email",
                Password = "Password123",
                ConfirmPassword = "Password123",
                SourcePage = "RegistrationPage"
            };

            var validationResults = ValidateModel(user, mockUserRepository.Object);

            Assert.Contains(validationResults, v => v.ErrorMessage == "Invalid email address.");
        }

        [Fact]
        public void PasswordMismatch_ShouldFailValidation()
        {
            var user = new LoginUser
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123",
                ConfirmPassword = "Password321",
                SourcePage = "RegistrationPage"
            };

            var validationResults = ValidateModel(user, mockUserRepository.Object);

            Assert.Contains(validationResults, v => v.ErrorMessage == "Passwords do not match.");
        }

        [Fact]
        public void UniqueEmail_Registration_ShouldFailValidation()
        {
            mockUserRepository.Setup(repo => repo.EmailExists(It.IsAny<string>())).Returns(true);

            var user = new LoginUser
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123",
                SourcePage = "RegistrationPage"
            };

            var validationResults = ValidateModel(user, mockUserRepository.Object);

            Assert.Contains(validationResults, v => v.ErrorMessage == "User with that Email already exists.");
        }

        [Fact]
        public void UniqueEmail_Login_ShouldFailValidation()
        {
            mockUserRepository.Setup(repo => repo.EmailExists(It.IsAny<string>())).Returns(false);

            var user = new LoginUser
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123",
                ConfirmPassword = "Password123",
                SourcePage = "LoginPage"
            };

            var validationResults = ValidateModel(user, mockUserRepository.Object);

            Assert.Contains(validationResults, v => v.ErrorMessage == "User with that Email does not exist.");
        }

        private IList<ValidationResult> ValidateModel(object model, IUserRepository userRepository)
        {
            var validationContext = new ValidationContext(model);
            if (userRepository != null)
            {
                validationContext.InitializeServiceProvider(serviceType => userRepository);
            }
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}