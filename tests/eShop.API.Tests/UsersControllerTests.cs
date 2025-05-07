using eShop.API.Controllers;
using eShop.Business.Interfaces;
using eShop.Shared.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace eShop.API.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UsersController _controller;
        private readonly List<UserDto> _users;

        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UsersController(_mockUserService.Object);

            // Setup test data
            _users = new List<UserDto>
            {
                new UserDto
                {
                    Id = "user1",
                    UserName = "testuser1",
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Roles = new List<string> { "Admin", "User" },
                    JoinedDate = "15/01/2023"
                },
                new UserDto
                {
                    Id = "user2",
                    UserName = "testuser2",
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    Roles = new List<string> { "User" },
                    JoinedDate = "20/03/2023"
                }
            };
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult()
        {
            // Arrange
            _mockUserService.Setup(service => service.GetAllAsync())
                .ReturnsAsync(_users);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllUsers()
        {
            // Arrange
            _mockUserService.Setup(service => service.GetAllAsync())
                .ReturnsAsync(_users);

            // Act
            var result = await _controller.GetAll();
            var okResult = result.Result as OkObjectResult;

            // Assert
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserDto>>(okResult.Value);
            var userList = Assert.IsType<List<UserDto>>(returnedUsers);
            Assert.Equal(2, userList.Count);
        }

        [Fact]
        public async Task GetAll_ShouldReturnCorrectUserData()
        {
            // Arrange
            _mockUserService.Setup(service => service.GetAllAsync())
                .ReturnsAsync(_users);

            // Act
            var result = await _controller.GetAll();
            var okResult = result.Result as OkObjectResult;
            var returnedUsers = okResult?.Value as IEnumerable<UserDto>;
            var userList = new List<UserDto>(returnedUsers);

            // Assert
            Assert.Equal("user1", userList[0].Id);
            Assert.Equal("John", userList[0].FirstName);
            Assert.Equal("Doe", userList[0].LastName);
            Assert.Equal("john.doe@example.com", userList[0].Email);
            Assert.Contains("Admin", userList[0].Roles);

            Assert.Equal("user2", userList[1].Id);
            Assert.Equal("Jane", userList[1].FirstName);
            Assert.Equal("Smith", userList[1].LastName);
            Assert.Equal("jane.smith@example.com", userList[1].Email);
            Assert.Contains("User", userList[1].Roles);
        }
    }
}