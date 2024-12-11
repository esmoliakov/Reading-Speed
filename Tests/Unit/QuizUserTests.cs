using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Server.Controllers;
using Server.Database;
using Server.Exceptions;
using Server.Services;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class ControllerTests
{
    private readonly Mock<IWebHostEnvironment> _mockEnvironment;
    private readonly Mock<ILogger<QuizController>> _mockQuizLogger;
    private readonly Mock<ILogger<UserController>> _mockUserLogger;
    private readonly Mock<ReadingSpeedDbContext> _mockDbContext;
    private readonly QuizController _quizController;
    private readonly UserController _userController;

    public ControllerTests()
    {
        _mockEnvironment = new Mock<IWebHostEnvironment>();
        _mockQuizLogger = new Mock<ILogger<QuizController>>();
        _mockUserLogger = new Mock<ILogger<UserController>>();

        // Mock DbContext with constructor arguments
        var options = new DbContextOptionsBuilder<ReadingSpeedDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new ReadingSpeedDbContext(options);
        // Then perform database operations directly.

        _mockDbContext = new Mock<ReadingSpeedDbContext>(options);

        _mockEnvironment.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());

        _quizController = new QuizController(_mockEnvironment.Object, _mockQuizLogger.Object, _mockDbContext.Object);
        _userController = new UserController(_mockEnvironment.Object);
    }

    // *** UserController Tests ***

    [Fact]
    public void SaveUser_ShouldReturnOk_WhenValidData()
    {
        // Arrange
        var username = "testuser1";
        var quizScore = 85;

        // Act
        var result = _userController.SaveUser(username, quizScore);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("User record saved successfully.", okResult.Value);
    }

    [Fact]
    public void SaveUser_ShouldReturnBadRequest_WhenUsernameAlreadyExists()
    {
        // Arrange
        var username = "testuser1";
        var quizScore = 85;

        // Simulate an existing user by pre-adding a record to `userRecords`
        UserDataService.SaveUserRecord(username, quizScore, "UserRecord.json");

        // Act
        var result = _userController.SaveUser(username, quizScore);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result); // Assert it is a BadRequest
        Assert.Equal("A user with this username already exists.", badRequestResult.Value); // Assert the message

        // Clean up for the test
        UserDataService.ClearUserRecords();
    }

    [Fact]
    public void ResetData_ShouldReturnOk_WhenResetIsSuccessful()
    {
        // Arrange
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "UserRecord.json");

        // Create a dummy file to simulate existing data
        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Files"));
        File.WriteAllText(filePath, "Dummy data");

        // Act
        var result = _userController.ResetData();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Data reset successfully.", okResult.Value);

        // Ensure the file has been deleted
        Assert.False(File.Exists(filePath));
    }

    [Fact]
    public void ResetData_ShouldReturnServerError_WhenExceptionOccurs()
    {
        // Arrange
        // Force an exception by mocking an invalid ContentRootPath
        _mockEnvironment.Setup(env => env.ContentRootPath).Throws(new Exception("Unexpected error"));

        // Recreate the controller with the mocked environment
        var controller = new UserController(_mockEnvironment.Object);

        // Act
        var result = controller.ResetData();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("An error occurred while resetting data.", statusCodeResult.Value);
    }
    [Fact]
    public void SaveUser_ShouldReturnBadRequest_WhenUsernameIsEmpty()
    {
        // Arrange
        var emptyUsername = ""; // Simulating an empty username
        var quizScore = 85;

        // Act
        var result = _userController.SaveUser(emptyUsername, quizScore);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result); // Ensure it's a BadRequest
        Assert.Equal("Username cannot be null or empty.", badRequestResult.Value); // Ensure the exception message matches the expected message
    }
    

}
