using Backend.DataAccess.Dtos;

namespace Backend.Tests.RepositoryTests
{
    public class SqlUserRepositoryTests
    {
        private readonly Mock<IDataSource> _mockDataSource;
        private readonly SqlUserRepository _repository;

        public SqlUserRepositoryTests()
        {
            _mockDataSource = new Mock<IDataSource>();
            _repository = new SqlUserRepository(_mockDataSource.Object);
        }

        [Fact]
        public void AddUser_ShouldExecuteNonQuery()
        {
            // Arrange
            string name = "John Doe";
            string mail = "john.doe@example.com";

            var sql = "INSERT INTO USERS (U_NAME, U_MAIL) VALUES (@name, @mail)";
            var parameters = new (string, object)[] { ("@name", name), ("@mail", mail) };

            // Act
            _repository.AddUser(name, mail);

            // Assert
            _mockDataSource.Verify(ds => ds.ExecuteNonQuery(sql, parameters), Times.Once);
        }

        [Fact]
        public void InsertUserCredentials_ShouldExecuteNonQuery()
        {
            // Arrange
            byte[] mailHash = new byte[] { 1, 2, 3 };
            byte[] passHash = new byte[] { 4, 5, 6 };

            var sql = "INSERT INTO USER_CREDENTIALS (UMAIL_HASH, UPASS_HASH) VALUES (@mailHash, @passHash)";
            var parameters = new (string, object)[] { ("@mailHash", mailHash), ("@passHash", passHash) };

            // Act
            _repository.InsertUserCredentials(mailHash, passHash);

            // Assert
            _mockDataSource.Verify(ds => ds.ExecuteNonQuery(sql, parameters), Times.Once);
        }

        [Fact]
        public void ModifyUser_ShouldExecuteNonQuery()
        {
            // Arrange
            int id = 1;
            string name = "Jane Doe";
            string mail = "jane.doe@example.com";

            var sql = "UPDATE USERS SET U_NAME = @name, U_MAIL = @mail WHERE U_ID = @id";
            var parameters = new (string, object)[] { ("@id", id), ("@name", name), ("@mail", mail) };

            // Act
            _repository.ModifyUser(id, name, mail);

            // Assert
            _mockDataSource.Verify(ds => ds.ExecuteNonQuery(sql, parameters), Times.Once);
        }

        [Fact]
        public void ModifyUserCredentials_ShouldExecuteNonQuery()
        {
            // Arrange
            byte[] mailHash = new byte[] { 1, 2, 3 };
            byte[] passHash = new byte[] { 4, 5, 6 };

            var sql = "UPDATE USER_CREDENTIALS SET UPASS_HASH = @passHash WHERE UMAIL_HASH = @mailHash";
            var parameters = new (string, object)[] { ("@mailHash", mailHash), ("@passHash", passHash) };

            // Act
            _repository.ModifyUserCredentials(mailHash, passHash);

            // Assert
            _mockDataSource.Verify(ds => ds.ExecuteNonQuery(sql, parameters), Times.Once);
        }

        [Fact]
        public void DeleteUser_ShouldExecuteNonQuery()
        {
            // Arrange
            int id = 1;

            var sql = "DELETE FROM USERS WHERE U_ID = @id";
            var parameters = new (string, object)[] { ("@id", id) };

            // Act
            _repository.DeleteUser(id);

            // Assert
            _mockDataSource.Verify(ds => ds.ExecuteNonQuery(sql, parameters), Times.Once);
        }

        [Fact]
        public void DeleteUserCredentials_ShouldExecuteNonQuery()
        {
            // Arrange
            byte[] mailHash = new byte[] { 1, 2, 3 };
            var sql = "DELETE FROM USER_CREDENTIALS WHERE UMAIL_HASH = @mailHash";
            var parameters = new (string, object)[] { ("@mailHash", mailHash) };

            // Act
            _repository.DeleteUserCredentials(mailHash);

            // Assert
            _mockDataSource.Verify(ds => ds.ExecuteNonQuery(sql, parameters), Times.Once);
        }

        [Fact]
        public void GetUsers_ShouldReturnListOfUsers()
        {
            // Arrange
            var users = new List<IUserDto>
            {
                new UserDto(1, "John Doe", "john.doe@example.com", false),
                new UserDto(2, "Jane Doe", "jane.doe@example.com", true)
            };

            _mockDataSource.Setup(ds => ds.ExecuteReader(It.IsAny<string>()))
                .Returns(new MockUserDataReader(users.Select(u => new object[] { u.Id, u.Name, u.Mail, u.IsAdmin }))); 

            // Act
            var result = _repository.GetUsers();

            // Assert
            result.Should().BeEquivalentTo(users);
        }

        [Fact]
        public void GetSingleUser_ShouldReturnUser()
        {
            // Arrange
            int id = 1;
            var user = new UserDto(id, "John Doe", "john.doe@example.com", false);

            _mockDataSource.Setup(ds => ds.ExecuteReader(It.IsAny<string>()))
                .Returns(new MockUserDataReader(new List<object[]> { new object[] { user.Id, user.Name, user.Mail, user.IsAdmin } }));

            // Act
            var result = _repository.GetSingleUser(id);

            // Assert
            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public void GetUserId_ShouldReturnUserId()
        {
            // Arrange
            string email = "john.doe@example.com";
            int userId = 1;

            _mockDataSource.Setup(ds => ds.ExecuteReader(It.IsAny<string>()))
                .Returns(new MockUserDataReader(new List<object[]> { new object[] { userId, "John Doe", "john.doe@example.com", false } }));

            // Act
            var result = _repository.GetUserId(email);

            // Assert
            result.Should().Be(userId);
        }

        [Fact]
        public void GetHashedPassword_ShouldReturnHashedPassword()
        {
            // Arrange
            string hashedEmailHex = "abcdef";
            byte[] hashedPasswordBytes = new byte[] { 0x12, 0x34, 0x56 };
            string hashedPassword = "123456";

            var mockDataReader = new Mock<IDataReader>();
            mockDataReader.SetupSequence(dr => dr.Read())
                .Returns(true)
                .Returns(false);
            mockDataReader.Setup(dr => dr["upass_hash"]).Returns(hashedPasswordBytes);

            _mockDataSource.Setup(ds => ds.ExecuteReader(It.IsAny<string>(), It.IsAny<(string, object)[]>()))
                .Returns(mockDataReader.Object);

            // Act
            var result = _repository.GetHashedPassword(hashedEmailHex);

            // Assert
            result.Should().Be(hashedPassword);
        }

        [Fact]
        public void EmailExists_ShouldReturnTrueIfEmailExists()
        {
            // Arrange
            string email = "john.doe@example.com";
            var mockReader = new Mock<IDataReader>();

            mockReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(false); 

            mockReader.Setup(r => r.GetString(0)).Returns(email);

            _mockDataSource.Setup(ds => ds.ExecuteReader(It.IsAny<string>()))
                .Returns(mockReader.Object);

            // Act
            var result = _repository.EmailExists(email);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsAdmin_ShouldReturnTrueIfUserIsAdmin()
        {
            // Arrange
            int userId = 1;
            bool isAdmin = true;
            var mockReader = new Mock<IDataReader>();

            mockReader.SetupSequence(r => r.Read())
                .Returns(true)  
                .Returns(false); 

            mockReader.Setup(r => r.GetBoolean(0)).Returns(isAdmin);

            _mockDataSource.Setup(ds => ds.ExecuteReader(It.IsAny<string>()))
                .Returns(mockReader.Object);
            

            // Act
            var result = _repository.IsAdmin(userId);

            // Assert
            result.Should().BeTrue();
        }
    }
}