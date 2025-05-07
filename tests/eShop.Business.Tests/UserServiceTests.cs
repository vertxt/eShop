using eShop.Business.Services;
using eShop.Data.Entities.UserAggregate;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace eShop.Business.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly UserService _userService;
        private readonly List<User> _users;

        public UserServiceTests()
        {
            // Setup mock store for UserManager
            var userStoreMock = new Mock<IUserStore<User>>();

            // Create UserManager mock
            _mockUserManager = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            // Initialize the service with the mock
            _userService = new UserService(_mockUserManager.Object);

            // Setup test data
            _users = new List<User>
            {
                new User
                {
                    Id = "user1",
                    UserName = "testuser1",
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    JoinedDate = new DateTime(2023, 1, 15)
                },
                new User
                {
                    Id = "user2",
                    UserName = "testuser2",
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    JoinedDate = new DateTime(2023, 3, 20)
                }
            };
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            _mockUserManager.Setup(m => m.Users).Returns(_users.AsQueryable());
            _mockUserManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) => user.Id == "user1"
                    ? new List<string> { "Admin", "User" }
                    : new List<string> { "User" });

            // Act
            var result = await _userService.GetAllAsync();
            var userDtos = result.ToList();

            // Assert
            Assert.Equal(2, userDtos.Count);
            Assert.Contains(userDtos, u => u.Id == "user1");
            Assert.Contains(userDtos, u => u.Id == "user2");
        }

        [Fact]
        public async Task GetAllAsync_ShouldMapUserPropertiesCorrectly()
        {
            // Arrange
            _mockUserManager.Setup(m => m.Users).Returns(_users.AsQueryable());
            _mockUserManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { "User" });

            // Act
            var result = await _userService.GetAllAsync();
            var userDto = result.First(u => u.Id == "user1");

            // Assert
            Assert.Equal("user1", userDto.Id);
            Assert.Equal("testuser1", userDto.UserName);
            Assert.Equal("John", userDto.FirstName);
            Assert.Equal("Doe", userDto.LastName);
            Assert.Equal("john.doe@example.com", userDto.Email);
            Assert.Equal("15/01/2023", userDto.JoinedDate);
        }

        [Fact]
        public async Task GetAllAsync_ShouldIncludeUserRoles()
        {
            // Arrange
            _mockUserManager.Setup(m => m.Users).Returns(_users.AsQueryable());
            _mockUserManager.Setup(m => m.GetRolesAsync(It.Is<User>(u => u.Id == "user1")))
                .ReturnsAsync(new List<string> { "Admin", "User" });
            _mockUserManager.Setup(m => m.GetRolesAsync(It.Is<User>(u => u.Id == "user2")))
                .ReturnsAsync(new List<string> { "User" });

            // Act
            var result = await _userService.GetAllAsync();
            var userDtos = result.ToList();

            // Assert
            var user1 = userDtos.First(u => u.Id == "user1");
            var user2 = userDtos.First(u => u.Id == "user2");

            Assert.Equal(2, user1.Roles.Count);
            Assert.Contains("Admin", user1.Roles);
            Assert.Contains("User", user1.Roles);

            Assert.Single(user2.Roles);
            Assert.Contains("User", user2.Roles);
        }
    }
}