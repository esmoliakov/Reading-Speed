using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Server.Controllers;
using Server.Database;
using Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Reflection;
using System.Diagnostics;
public class TimerControllerTests
{
    private readonly TimerController _controller;
    private readonly ReadingSpeedDbContext _context;

    public TimerControllerTests()
    {
        var options = new DbContextOptionsBuilder<ReadingSpeedDbContext>()
            .UseInMemoryDatabase(databaseName: "ReadingSpeedTestDb") // Using an in-memory database
            .Options;

        _context = new ReadingSpeedDbContext(options);
        _controller = new TimerController(_context);
    }

    [Fact]
    public async Task FindBestTime_ReturnsBestTime_WhenUserExists()
    {
        // Arrange
        var userName = "test_user";
        _context.Attempts.AddRange(
            new AttemptEntity { UserName = userName, ReadingTime = 1500, Id = 1 },
            new AttemptEntity { UserName = userName, ReadingTime = 1200, Id = 2 }, // Best time
            new AttemptEntity { UserName = userName, ReadingTime = 1800, Id = 3 }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.FindBestTime(userName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(1200L, okResult.Value); // Best time should be 1200
    }

    [Fact]
    public async Task FindBestTime_ReturnsBadRequest_WhenUserHasNoAttempts()
    {
        // Arrange
        var userName = "non_existent_user";

        // Act
        var result = await _controller.FindBestTime(userName);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal($"User {userName} does not exist.", badRequestResult.Value);
    }

    [Fact]
    public async Task FindBestTime_ReturnsBadRequest_WhenUserNameIsNullOrEmpty()
    {
        // Act
        var result = await _controller.FindBestTime(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("User  does not exist.", badRequestResult.Value);
    }

    [Fact]
    public async Task StopTimer_ReturnsBadRequest_WhenStopwatchIsNotRunning()
    {
        // Using reflection to access the private static Stopwatch field
        var stopwatchField = typeof(TimerController).GetField("stopwatch", BindingFlags.NonPublic | BindingFlags.Static);
        var stopwatch = (Stopwatch)stopwatchField.GetValue(null);

        // Reset the stopwatch to ensure it's not running
        stopwatch.Reset();

        // Act: Call StopTimer when the stopwatch is not running
        var result = await _controller.StopTimer();

        // Assert: Verify that a BadRequestObjectResult is returned with the correct message
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Stopwatch is not running.", badRequestResult.Value);
    }

}
