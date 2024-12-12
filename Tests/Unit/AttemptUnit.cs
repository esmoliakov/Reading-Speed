using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Controllers;
using Server.Database;
using Server.Services;
using Shared.Models;
using Shared.Models.DTOs;
using Xunit;

public class AttemptControllerIntegrationTests
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
            Id = 1,
            UserName = "Jane Doe",
            ReadingTime = 200,
            Wpm = 150,
            ParagraphId = 1,
            Score = 90
        };
        await context.Attempts.AddAsync(attempt);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.GetAttempt(1);

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
            Id = 1,
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
}
