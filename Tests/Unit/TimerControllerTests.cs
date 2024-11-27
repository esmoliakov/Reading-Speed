using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.Controllers;
using Server.Services;
using Xunit;

public class FileControllerTests
{
    private readonly FileController _controller;
    private readonly Mock<IWebHostEnvironment> _mockEnv;
    private readonly string _baseFilePath;

    public FileControllerTests()
    {
        _mockEnv = new Mock<IWebHostEnvironment>();
        _mockEnv.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
        _controller = new FileController(_mockEnv.Object);
        _baseFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
    }

    [Fact]
    public void ReadTextFile_ShouldReturnOk_WhenFileExists()
    {
        // Arrange
        var fileName = "sample_text.txt";
        var filePath = Path.Combine(_baseFilePath, fileName);
        Directory.CreateDirectory(_baseFilePath);
        File.WriteAllText(filePath, "Sample file content.");

        // Act
        var result = _controller.ReadTextFile(fileName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Sample file content.", okResult.Value);
    }

    [Fact]
    public void ReadTextFile_ShouldReturnNotFound_WhenFileDoesNotExist()
    {
        // Arrange
        var fileName = "non_existent_file.txt";

        // Act
        var result = _controller.ReadTextFile(fileName);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void ReadLastFileLine_ShouldReturnOk_WhenFileExists()
    {
        // Arrange
        var fileName = "sample_lines.txt";
        var filePath = Path.Combine(_baseFilePath, fileName);
        Directory.CreateDirectory(_baseFilePath);
        File.WriteAllLines(filePath, new[] { "Line 1", "Line 2", "Line 3" });

        // Act
        var result = _controller.ReadLastFileLine(fileName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Line 3", okResult.Value);
    }

    [Fact]
    public void ReadLastFileLine_ShouldReturnNotFound_WhenFileDoesNotExist()
    {
        // Arrange
        var fileName = "non_existent_lines.txt";

        // Act
        var result = _controller.ReadLastFileLine(fileName);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void GetParagraphId_ShouldReturnOk_WhenFileExists()
    {
        // Arrange
        var filePath = Path.Combine(_baseFilePath, "paragraphId.txt");
        Directory.CreateDirectory(_baseFilePath);
        File.WriteAllText(filePath, "123");

        // Act
        var result = _controller.GetParagrapghId();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("123", okResult.Value);
    }

    [Fact]
    public void GetParagraphId_ShouldReturnNotFound_WhenFileDoesNotExist()
    {
        // Arrange
        var filePath = Path.Combine(_baseFilePath, "paragraphId.txt");
        if (File.Exists(filePath)) File.Delete(filePath); // Ensure file is absent

        // Act
        var result = _controller.GetParagrapghId();

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}