using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Controllers;
using Server.Database;
using Shared.Interfaces;
using Shared.Models;
using Shared.Models.DTOs;
using Xunit;

namespace Server.Tests
{
    public class QuestionsControllerTests
    {
        private readonly Mock<IRepository<QuestionEntity>> _mockQuestionRepo;
        private readonly Mock<IRepository<ParagraphEntity>> _mockParagraphRepo;
        private readonly ReadingSpeedDbContext _dbContext;
        private readonly QuestionsController _controller;

        public QuestionsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ReadingSpeedDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _dbContext = new ReadingSpeedDbContext(options);

            _mockQuestionRepo = new Mock<IRepository<QuestionEntity>>();
            _mockParagraphRepo = new Mock<IRepository<ParagraphEntity>>();

            _controller = new QuestionsController(_dbContext);
        }

        [Fact]
        public async Task GetQuestions_ReturnsNotFound_WhenNoQuestionsFound()
        {
            // Arrange
            var paragraphId = 1;

            _dbContext.Questions.RemoveRange(_dbContext.Questions);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.GetQuestions(paragraphId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No questions found for Paragraph ID 1.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetQuestions_ReturnsOk_WhenQuestionsFound()
        {
            // Arrange
            var paragraphId = 1;
            var question = new QuestionEntity
            {
                Id = 1,
                ParagraphId = paragraphId,
                Text = "Sample question"
            };

            _dbContext.Questions.Add(question);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.GetQuestions(paragraphId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<QuestionEntity>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task DeleteQuestion_ReturnsNotFound_WhenQuestionDoesNotExist()
        {
            // Arrange
            var questionId = 1;
            _mockQuestionRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((QuestionEntity)null);

            // Act
            var result = await _controller.DeleteQuestion(questionId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Question with ID 1 was not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteQuestion_ReturnsNoContent_WhenQuestionIsDeleted()
        {
            // Arrange
            var questionId = 1;
            var question = new QuestionEntity { Id = questionId, Text = "Sample question" };
            _mockQuestionRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(question);
            _mockQuestionRepo.Setup(repo => repo.DeleteAsync(It.IsAny<QuestionEntity>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteQuestion(questionId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
