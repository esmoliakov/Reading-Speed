using Shared.Models;
using System.Text.Json;
using Xunit;

namespace Server.Tests
{
    public class QuizSubmissionTests
    {

        [Fact]
        public void QuizSubmission_ShouldContainQuestions()
        {
            // Arrange
            var quizSubmission = new QuizSubmission
            {
                UserName = "TestUser",
                Filename = "testFile.txt",
                Questions = new List<Question>
                {
                    new Question { Id = 1, Text = "What is 2 + 2?", Options = new List<string> { "3", "4", "5" }, CorrectAnswers = new List<string> { "4" }}
                }
            };

            // Act
            var questionCount = quizSubmission.Questions.Count;

            // Assert
            Assert.Equal(1, questionCount); // Ensure the quiz submission has exactly one question
        }
    }
}
