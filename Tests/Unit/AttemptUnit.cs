using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Controllers;
using Server.Database;
using Server.Services;
using Shared.Models;
using Shared.Models.DTOs;
using Xunit;

public class AttemptControllerTests
{
    private ReadingSpeedDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ReadingSpeedDbContext>()
            .UseInMemoryDatabase(databaseName: "AttemptTestDb")
            .Options;

        return new ReadingSpeedDbContext(options);
    }

    [Fact]
    public async Task GetAttempt_ShouldReturnCorrectAttempt()
    {
        // Arrange
        var context = CreateInMemoryDbContext();
        var quizService = new QuizService();
        var controller = new AttemptController(context, quizService);

        var attempt = new AttemptEntity
        {
            Id = 12,
            UserName = "Jane Doe",
            ReadingTime = 200,
            Wpm = 150,
            ParagraphId = 1,
            Score = 90
        };
        await context.Attempts.AddAsync(attempt);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.GetAttempt(12);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var fetchedAttempt = Assert.IsType<AttemptEntity>(okResult.Value);

        Assert.Equal(attempt.Id, fetchedAttempt.Id);
        Assert.Equal(attempt.UserName, fetchedAttempt.UserName);
    }

    [Fact]
    public async Task GetAttempt_ShouldReturnNotFound_WhenAttemptDoesNotExist()
    {
        // Arrange
        var context = CreateInMemoryDbContext();
        var quizService = new QuizService();
        var controller = new AttemptController(context, quizService);

        // Act
        var result = await controller.GetAttempt(999);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Attempt with id 999 does not exist", notFoundResult.Value);
    }


[Fact]
public async Task AddAttempt_ShouldReturnBadRequest_WhenParagraphNotFound()
{
    // Arrange
    var context = CreateInMemoryDbContext();
    var quizService = new QuizService();
    var controller = new AttemptController(context, quizService);

    // Create a list of UserAnswerDTO with sample Question and UserAnswer values
    var userAnswers = new List<UserAnswerDTO>
    {
        new UserAnswerDTO
        {
            Question = new Question { Id = 1, Text = "What is 2 + 2?" },  // Example Question
            UserAnswer = "4"  // Example Answer
        },
        new UserAnswerDTO
        {
            Question = new Question { Id = 2, Text = "What is the capital of France?" },
            UserAnswer = "Paris"
        },
        new UserAnswerDTO
        {
            Question = new Question { Id = 3, Text = "What is the color of the sky?" },
            UserAnswer = "Blue"
        }
    };

    var createAttemptDto = new CreateAttemptDTO
    {
        UserName = "John Doe",
        ReadingTime = 200,
        ParagraphId = 999, // Invalid ParagraphId
        UserAnswers = userAnswers // Now passing List<UserAnswerDTO>
    };

    // Act
    var result = await controller.AddAttempt(createAttemptDto);

    // Assert
    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    Assert.Equal("Paragraph with id 999 does not exist", badRequestResult.Value);
}


    [Fact]
    public async Task DeleteAttempt_ShouldRemoveAttemptFromDatabase()
    {
        // Arrange
        var context = CreateInMemoryDbContext();
        var quizService = new QuizService();
        var controller = new AttemptController(context, quizService);

        var attempt = new AttemptEntity
        {
            Id = 10,
            UserName = "Test User",
            ReadingTime = 300,
            Wpm = 100,
            ParagraphId = 1,
            Score = 80
        };

        await context.Attempts.AddAsync(attempt);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.DeleteAttempt(attempt.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);

        var deletedAttempt = await context.Attempts.FirstOrDefaultAsync(a => a.Id == attempt.Id);
        Assert.Null(deletedAttempt);
    }

    [Fact]
    public async Task DeleteAttempt_ShouldReturnNotFound_WhenAttemptDoesNotExist()
    {
        // Arrange
        var context = CreateInMemoryDbContext();
        var quizService = new QuizService();
        var controller = new AttemptController(context, quizService);

        // Act
        var result = await controller.DeleteAttempt(999);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Attempt with id 999 does not exist", notFoundResult.Value);
    }
    
    [Fact]
    public async Task GetUsersBestWpms_ShouldReturnTopWpms_WhenUserHasAttempts()
    {
        // Arrange
        var context = CreateInMemoryDbContext();
        var quizService = new QuizService();
        var controller = new AttemptController(context, quizService);

        var attempts = new List<AttemptEntity>
        {
            new AttemptEntity { Id = 1, UserName = "JohnDoe", ReadingTime = 200, Wpm = 180, Score = 85, ParagraphId = 1 },
            new AttemptEntity { Id = 2, UserName = "JohnDoe", ReadingTime = 150, Wpm = 170, Score = 75, ParagraphId = 1 },
            new AttemptEntity { Id = 3, UserName = "JohnDoe", ReadingTime = 100, Wpm = 160, Score = 70, ParagraphId = 1 }
        };

        await context.Attempts.AddRangeAsync(attempts);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.GetUsersBestWpms("JohnDoe");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var fetchedAttempts = Assert.IsType<List<AttemptEntity>>(okResult.Value);

        // Check the number of returned attempts and their order
        Assert.Equal(3, fetchedAttempts.Count);  // Expecting top 3 attempts by WPM
        Assert.True(fetchedAttempts[0].Wpm > fetchedAttempts[1].Wpm);  // Check that they are sorted by WPM
    }
    [Fact]
    public async Task GetUsersBestScores_ShouldReturnTopScores_WhenUserHasAttempts()
    {
        // Arrange
        var context = CreateInMemoryDbContext();
        var quizService = new QuizService();
        var controller = new AttemptController(context, quizService);

        var attempts = new List<AttemptEntity>
        {
            new AttemptEntity { UserName = "JohnDoe", ReadingTime = 200, Wpm = 180, Score = 90, ParagraphId = 1 },
            new AttemptEntity { UserName = "JohnDoe", ReadingTime = 150, Wpm = 170, Score = 85, ParagraphId = 1 },
            new AttemptEntity { UserName = "JohnDoe", ReadingTime = 100, Wpm = 160, Score = 80, ParagraphId = 1 },
            new AttemptEntity { UserName = "JohnDoe", ReadingTime = 120, Wpm = 175, Score = 95, ParagraphId = 1 },
            new AttemptEntity { UserName = "JohnDoe", ReadingTime = 140, Wpm = 165, Score = 88, ParagraphId = 1 },
            new AttemptEntity { UserName = "JohnDoe", ReadingTime = 130, Wpm = 150, Score = 70, ParagraphId = 1 }
        };


        await context.Attempts.AddRangeAsync(attempts);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.GetUsersBestScores("JohnDoe");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var fetchedAttempts = Assert.IsType<List<AttemptEntity>>(okResult.Value);

        // Check that we have 5 attempts (top 5 by score)
        Assert.Equal(5, fetchedAttempts.Count);

        // Check that they are sorted by score in descending order
        Assert.True(fetchedAttempts[0].Score >= fetchedAttempts[1].Score);
        Assert.True(fetchedAttempts[1].Score >= fetchedAttempts[2].Score);
        Assert.True(fetchedAttempts[2].Score >= fetchedAttempts[3].Score);
        Assert.True(fetchedAttempts[3].Score >= fetchedAttempts[4].Score);
    }

    [Fact]
    public async Task GetUsersBestScores_ShouldReturnNotFound_WhenNoAttemptsFound()
    {
        // Arrange
        var context = CreateInMemoryDbContext();
        var quizService = new QuizService();
        var controller = new AttemptController(context, quizService);

        // Act
        var result = await controller.GetUsersBestScores("NonExistentUser");

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No attempts found", notFoundResult.Value);
    }

}
