using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using DLS.Server.Controllers;
using DLS.Server.Services;
using DLS.Server.Models;

namespace DLS.Server.AuthControllerUnitTest
{
    public class AuthControllerTests
    {
        private readonly Mock<AuthService> _mockAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<AuthService>(Mock.Of<IServiceProvider>());
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public void Login_ReturnsOkResult_WhenCredentialsAreValid()
        {
            // Arrange
            var loginRequest = new LoginRequest { Username = "test", Password = "password" };
            _mockAuthService.Setup(service => service.GenerateToken(It.IsAny<string>())).Returns("valid-token");

            // Act
            var result = _controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dynamic>(okResult.Value);
            Assert.Equal("valid-token", response.Token);
        }

        [Fact]
        public void Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginRequest = new LoginRequest { Username = "wrong", Password = "wrong" };

            // Act
            var result = _controller.Login(loginRequest);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid credentials", unauthorizedResult.Value);
        }
    }
}

