using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using DLS.Server.Controllers;
using DLS.Server.Services;
using DLS.Server.Models;

namespace DLS.Server.UnitTest
{
    public class LogsControllerTests
    {
        private readonly Mock<IStorageBackend> _mockLogService;
        private readonly LogsController _controller;

        public LogsControllerTests()
        {
            _mockLogService = new Mock<IStorageBackend>();
            _controller = new LogsController(_mockLogService.Object);
        }

        [Fact]
        public void GetLogs_ReturnsOkResult_WhenLogsExist()
        {
            // Arrange
            var logEntries = new List<LogEntry>
            {
                new LogEntry { Id = new Guid(), Message = "Test log 1" },
                new LogEntry { Id = new Guid(), Message = "Test log 2" }
            };

            _mockLogService.Setup(service => service.RetrieveLogsAsync(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((0, new List<LogEntry>()));

            // Act
            var result = _controller.GetLogs("", "", null, null, 1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var logs = Assert.IsType<List<LogEntry>>(okResult.Value);
            Assert.Equal(2, logs.Count);
        }

        [Fact]
        public void GetLogs_ReturnsEmptyList_WhenNoLogsExist()
        {
            // Arrange
            _mockLogService.Setup(service => service.RetrieveLogsAsync(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((0, new List<LogEntry>()));

            // Act
            var result = _controller.GetLogs("", "", null, null, 1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var logs = Assert.IsType<List<LogEntry>>(okResult.Value);
            Assert.Empty(logs);
        }
    }
}
