using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.Services;
using Xunit;
using Server.Controllers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Threading.Tasks;

public class TimerControllerTests
{
    private readonly TimerController _controller;
    private readonly Mock<IWebHostEnvironment> _mockEnv;

    public TimerControllerTests()
    {
        _mockEnv = new Mock<IWebHostEnvironment>();
        _mockEnv.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
        _controller = new TimerController(_mockEnv.Object);
    }

    [Fact]
    public void StartTimer_ShouldReturnOk()
    {
        // Act
        var result = _controller.StartTimer();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task StopTimer_ShouldReturnElapsedTime()
    {
        // Arrange
        _controller.StartTimer();
        await Task.Delay(500); // Wait for some time to simulate elapsed time

        // Act
        var result = await _controller.StopTimer();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<long>(okResult.Value);
    }

    [Fact]
    public void WriteToFile_ShouldWriteTimeToFile()
    {
        // Arrange
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "stopwatch.txt");
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));  // Ensure the directory exists
        }
        // You can create an empty file for testing purposes
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");  // Create the file if it doesn't exist
        }

        // Act
        var result = _controller.WriteToFile(1000);

        // Assert
        Assert.IsType<OkResult>(result);
    }

}
