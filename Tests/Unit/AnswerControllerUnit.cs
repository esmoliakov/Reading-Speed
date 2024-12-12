using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Controllers;
using Server.Database;
using Shared.Models;
using Shared.Models.DTOs;
using Xunit;

public class AnswerControllerTests
{
    private ReadingSpeedDbContext CreateInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ReadingSpeedDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new ReadingSpeedDbContext(options);
    }

    [Fact]
    public async Task CreateAnswer_ShouldAddAnswerToDatabase()
    {
        // Arrange
        var context = CreateInMemoryDbContext("TestDb_CreateAnswer");
        var controller = new AnswerController(context);

        var question = new QuestionEntity { Id = 10, Text = "Sample question text" };
        await context.Questions.AddAsync(question);
        await context.SaveChangesAsync();

        var answerDto = new AnswerDTO
        {
            QuestionId = question.Id,
            Answer = "Sample Answer",
            IsCorrectAnswer = true
        };

        // Act
        var result = await controller.CreateAnswer(answerDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<AnswerEntity>>(result);
        Assert.IsType<OkResult>(actionResult.Result);

        var addedAnswer = await context.Answers.FirstOrDefaultAsync();
        Assert.NotNull(addedAnswer);
        Assert.Equal("Sample Answer", addedAnswer.Answer);
        Assert.True(addedAnswer.IsCorrectAnswer);
    }

    [Fact]
    public async Task CreateAnswers_ShouldAddMultipleAnswersToDatabase()
    {
        // Arrange
        var context = CreateInMemoryDbContext("TestDb_CreateAnswers");
        var controller = new AnswerController(context);

        var question = new QuestionEntity { Id = 20, Text = "Sample question text" };
        await context.Questions.AddAsync(question);
        await context.SaveChangesAsync();

        var answersDto = new List<AnswerDTO>
        {
            new AnswerDTO { QuestionId = question.Id, Answer = "Answer 1", IsCorrectAnswer = false },
            new AnswerDTO { QuestionId = question.Id, Answer = "Answer 2", IsCorrectAnswer = true }
        };

        // Act
        var result = await controller.CreateAnswer(answersDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<AnswerEntity>>(result);
        Assert.IsType<OkResult>(actionResult.Result);

        var addedAnswers = await context.Answers.Where(a => a.QuestionId == question.Id).ToListAsync();
        Assert.Equal(2, addedAnswers.Count); // Verify only relevant answers
        Assert.Contains(addedAnswers, a => a.Answer == "Answer 1");
        Assert.Contains(addedAnswers, a => a.Answer == "Answer 2");
    }

    [Fact]
    public async Task GetAnswers_ShouldReturnListOfAnswers()
    {
        // Arrange
        var context = CreateInMemoryDbContext("TestDb_GetAnswers");
        var controller = new AnswerController(context);

        var question = new QuestionEntity { Id = 3, Text = "Sample question text" };
        await context.Questions.AddAsync(question);
        await context.SaveChangesAsync();

        var answers = new List<AnswerEntity>
        {
            new AnswerEntity { QuestionId = question.Id, Answer = "Answer 1", IsCorrectAnswer = false },
            new AnswerEntity { QuestionId = question.Id, Answer = "Answer 2", IsCorrectAnswer = true }
        };
        await context.Answers.AddRangeAsync(answers);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.GetAnswers(question.Id);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var returnedAnswers = Assert.IsType<List<string>>(actionResult.Value);
        Assert.Equal(2, returnedAnswers.Count);
        Assert.Contains("Answer 1", returnedAnswers);
        Assert.Contains("Answer 2", returnedAnswers);
    }

    [Fact]
    public async Task DeleteAnswer_ShouldRemoveAnswerFromDatabase()
    {
        // Arrange
        var context = CreateInMemoryDbContext("TestDb_DeleteAnswer");
        var controller = new AnswerController(context);

        var answer = new AnswerEntity { Id = 11, QuestionId = 11, Answer = "Answer to be deleted", IsCorrectAnswer = false };
        await context.Answers.AddAsync(answer);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.DeleteAnswer(answer.Id);

        // Assert
        var actionResult = Assert.IsType<ActionResult<AnswerEntity>>(result);
        Assert.IsType<NoContentResult>(actionResult.Result); // Correctly check the result type

        var deletedAnswer = await context.Answers.FindAsync(answer.Id);
        Assert.Null(deletedAnswer);
    }
}
