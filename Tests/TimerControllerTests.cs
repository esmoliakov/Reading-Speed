using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Server.Controllers;
using Microsoft.AspNetCore.Hosting;
using Server.Services;
using System.IO;
using System.Threading.Tasks;

public class TimerControllerTests
{
    private readonly TimerController _controller;
    private readonly Mock<IWebHostEnvironment> _mockEnv;

    public TimerControllerTests()
    {
        // Mocking IWebHostEnvironment to avoid file path issues
        _mockEnv = new Mock<IWebHostEnvironment>();
        _mockEnv.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
        // Initialize the controller with mocked dependencies
        _controller = new TimerController(_mockEnv.Object);
    }

    [Fact]
    public void StartTimer_ShouldReturnOk()
    {
        // Act
        var result = _controller.StartTimer();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.Equal("Stopwatch started.", okResult.Value);
    }

    [Fact]
    public async Task StopTimer_ShouldReturnElapsedTime()
    {
        // Arrange
        var controller = new TimerController(_mockEnv.Object);
        controller.StartTimer();

        // 1 second run
        await Task.Delay(1000);

        var result = await controller.StopTimer();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var elapsedTime = (long)okResult.Value;
        Assert.InRange(elapsedTime, 900, 1100); // Check if elapsed time is approximately 1 second
    }

    [Fact]
    public void WriteToFile_ShouldReturnNotFound_WhenFileDoesNotExist()
    {
        // Arrange
        long? elapsedMilliseconds = 1000;
        var filePath = Path.Combine(_mockEnv.Object.ContentRootPath, "Files", "stopwatch.txt");

        _mockEnv.Setup(env => env.ContentRootPath).Returns("non-existing-path");

        // Act
        var result = _controller.WriteToFile(elapsedMilliseconds.Value);  // Use the Value property for nullable long

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Questions file not found.", notFoundResult.Value);
    }

    [Fact]
    public void WriteToFile_ShouldCallWriteTimeToFile_WhenFileExists()
    {
        // Arrange
        long? elapsedMilliseconds = 500;
        var filePath = Path.Combine(_mockEnv.Object.ContentRootPath, "Files", "stopwatch.txt");

        // Act
        var result = _controller.WriteToFile(elapsedMilliseconds.Value);  // Use the Value property for nullable long

        // Assert
        Assert.IsType<OkResult>(result);
        TimerServices.WriteTimeToFile(elapsedMilliseconds.Value, filePath);
    }

    [Fact]
    public void FindBestTime_ShouldReturnNotFound_WhenFileDoesNotExist()
    {
        // Arrange
        string fileName = "non-existing-file.txt";
        var filePath = Path.Combine(_mockEnv.Object.ContentRootPath, "Files", fileName);

        // Act
        var result = _controller.FindBestTime(fileName);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("File not found.", notFoundResult.Value);
    }

    [Fact]
    public void FindBestTime_ShouldReturnOk_WhenFileExists()
    {
        // Arrange
        string fileName = "stopwatch.txt";
        var filePath = Path.Combine(_mockEnv.Object.ContentRootPath, "Files", fileName);

        // Mock the method to return true, simulating that the file exists
        _mockEnv.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
        
        // Act
        var result = _controller.FindBestTime(fileName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("500", okResult.Value);
    }

}