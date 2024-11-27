using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.Controllers;
using Microsoft.AspNetCore.Hosting;
using Shared.Models;
using Xunit;

namespace Server.Tests
{
    public class QuestionsControllerTests
    {
        private readonly QuestionsController _controller;
        private readonly Mock<IWebHostEnvironment> _mockEnv;
        private readonly string _testFilePath;

        public QuestionsControllerTests()
        {
            _mockEnv = new Mock<IWebHostEnvironment>();
            _mockEnv.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            _controller = new QuestionsController(_mockEnv.Object);

            // Setting the file path for test questions file
            _testFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "test_questions.json");

            // Create a test questions file
            var questions = new List<Question>
            {
                new Question { id = 1, text = "What is the primary role of bees in agriculture?", options = new List<string> { "Option A", "Option B" }, correctAnswer = "Option A" },
                new Question { id = 2, text = "What is the habitat of bees?", options = new List<string> { "Option A", "Option B" }, correctAnswer = "Option B" }
            };

            // Serialize the questions and write to the test file
            var json = JsonSerializer.Serialize(questions);
            File.WriteAllText(_testFilePath, json);
        }

        [Fact]
        public void GetQuestions_ShouldReturnOk_WhenFileExists()
        {
            // Act
            var result = _controller.GetQuestions("test_questions.json");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Question>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public void GetQuestions_ShouldReturnNotFound_WhenFileDoesNotExist()
        {
            // Act
            var result = _controller.GetQuestions("non_existent_questions.json");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Questions file not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetQuestionsSorted_ShouldReturnSortedQuestions_WhenFileExists()
        {
            // Act
            var result = _controller.GetQuestionsSorted("test_questions.json");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Question>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            Assert.True(returnValue[0].text.CompareTo(returnValue[1].text) < 0);
        }

        [Fact]
        public void GetQuestionsSorted_ShouldReturnNotFound_WhenFileDoesNotExist()
        {
            // Act
            var result = _controller.GetQuestionsSorted("non_existent_questions.json");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Questions file not found.", notFoundResult.Value);
        }

        // Clean up after tests (delete the test file)
        public void Dispose()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }
    }
}