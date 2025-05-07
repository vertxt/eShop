using System.Text.Json;
using eShop.API.Middleware;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace eShop.API.Tests.Middlewares
{
    public class GlobalExceptionHandlerMiddlewareTests
    {
        private readonly Mock<ILogger<GlobalExceptionHandlerMiddleware>> _mockLogger;
        private readonly Mock<IWebHostEnvironment> _mockEnvironment;
        private readonly DefaultHttpContext _httpContext;
        private readonly GlobalExceptionHandlerMiddleware _middleware;

        public GlobalExceptionHandlerMiddlewareTests()
        {
            _mockLogger = new Mock<ILogger<GlobalExceptionHandlerMiddleware>>();
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            _httpContext = new DefaultHttpContext
            {
                Response = { Body = new MemoryStream() }
            };

            // Setup environment
            _mockEnvironment.Setup(e => e.EnvironmentName).Returns("Development");

            // Create the middleware with a RequestDelegate that throws an exception
            _middleware = new GlobalExceptionHandlerMiddleware(
                _ => throw new Exception("Test exception"),
                _mockLogger.Object,
                _mockEnvironment.Object);
        }

        [Fact]
        public async Task InvokeAsync_WithException_LogsError()
        {
            // Arrange

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            // Verify that logger.LogError was called
            _mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_WithException_SetsCorrectStatusCode()
        {
            // Arrange
            // Reset the stream position
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_WithKeyNotFoundException_Returns404()
        {
            // Arrange
            var middleware = new GlobalExceptionHandlerMiddleware(
                _ => throw new KeyNotFoundException("Not found"),
                _mockLogger.Object,
                _mockEnvironment.Object);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_WithArgumentException_Returns400()
        {
            // Arrange
            var middleware = new GlobalExceptionHandlerMiddleware(
                _ => throw new ArgumentException("Bad argument"),
                _mockLogger.Object,
                _mockEnvironment.Object);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_WithUnauthorizedAccessException_Returns401()
        {
            // Arrange
            var middleware = new GlobalExceptionHandlerMiddleware(
                _ => throw new UnauthorizedAccessException("Unauthorized"),
                _mockLogger.Object,
                _mockEnvironment.Object);

            // Act
            await middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status401Unauthorized, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_SetsContentTypeToApplicationProblemJson()
        {
            // Arrange

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal("application/problem+json", _httpContext.Response.ContentType);
        }

        [Fact]
        public async Task InvokeAsync_InDevEnvironment_IncludesExceptionDetail()
        {
            // Arrange
            _mockEnvironment.Setup(e => e.EnvironmentName).Returns("Development");
            _httpContext.Response.Body = new MemoryStream();

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(problemDetails);
            Assert.Equal("Test exception", problemDetails.Detail);
        }

        [Fact]
        public async Task InvokeAsync_InProdEnvironment_DoesNotIncludeExceptionDetail()
        {
            // Arrange
            _mockEnvironment.Setup(e => e.EnvironmentName).Returns("Production");
            _httpContext.Response.Body = new MemoryStream();

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(problemDetails);
            Assert.Null(problemDetails.Detail);
        }
    }
}