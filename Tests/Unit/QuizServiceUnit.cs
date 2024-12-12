using System;
using System.Collections.Generic;
using Shared.Models.DTOs;
using Xunit;
using Server.Services;
using Shared.Models;

namespace Server.Tests
{
    public class QuizServiceTests
    {
        private readonly QuizService _quizService;

        public QuizServiceTests()
        {
            _quizService = new QuizService();
        }

        [Fact]
        public void QuizScore_ShouldReturnCorrectScore_WhenUserAnswersAreCorrect()
        {
            // Arrange: Create a list of user answers with correct answers
            var userAnswers = new List<UserAnswerDTO>
            {
                new UserAnswerDTO
                {
                    Question = new Question { Id = 1, Text = "What is 2 + 2?", CorrectAnswers = new List<string> { "4" } },
                    UserAnswer = "4"
                },
                new UserAnswerDTO
                {
                    Question = new Question { Id = 2, Text = "What is the capital of France?", CorrectAnswers = new List<string> { "Paris" } },
                    UserAnswer = "Paris"
                },
                new UserAnswerDTO
                {
                    Question = new Question { Id = 3, Text = "What is the color of the sky?", CorrectAnswers = new List<string> { "Blue" } },
                    UserAnswer = "Blue"
                }
            };

            // Act: Call the QuizScore method
            var score = _quizService.QuizScore(userAnswers);

            // Assert: Check if the score is correct (100% in this case)
            Assert.Equal(100, score);
        }

        [Fact]
        public void QuizScore_ShouldReturnCorrectScore_WhenUserAnswersAreIncorrect()
        {
            // Arrange: Create a list of user answers with incorrect answers
            var userAnswers = new List<UserAnswerDTO>
            {
                new UserAnswerDTO
                {
                    Question = new Question { Id = 1, Text = "What is 2 + 2?", CorrectAnswers = new List<string> { "4" } },
                    UserAnswer = "5"
                },
                new UserAnswerDTO
                {
                    Question = new Question { Id = 2, Text = "What is the capital of France?", CorrectAnswers = new List<string> { "Paris" } },
                    UserAnswer = "London"
                },
                new UserAnswerDTO
                {
                    Question = new Question { Id = 3, Text = "What is the color of the sky?", CorrectAnswers = new List<string> { "Blue" } },
                    UserAnswer = "Green"
                }
            };

            // Act: Call the QuizScore method
            var score = _quizService.QuizScore(userAnswers);

            // Assert: Check if the score is 0% (since all answers are wrong)
            Assert.Equal(0, score);
        }

        [Fact]
        public void QuizScore_ShouldReturnPartialScore_WhenSomeUserAnswersAreCorrect()
        {
            // Arrange: Create a list of user answers with a mix of correct and incorrect answers
            var userAnswers = new List<UserAnswerDTO>
            {
                new UserAnswerDTO
                {
                    Question = new Question { Id = 1, Text = "What is 2 + 2?", CorrectAnswers = new List<string> { "4" } },
                    UserAnswer = "4"
                },
                new UserAnswerDTO
                {
                    Question = new Question { Id = 2, Text = "What is the capital of France?", CorrectAnswers = new List<string> { "Paris" } },
                    UserAnswer = "London"
                }
            };

            // Act: Call the QuizScore method
            var score = _quizService.QuizScore(userAnswers);

            // Assert: Check if the score is 50% (1 out of 2 answers are correct)
            Assert.Equal(50, score);
        }

        [Fact]
        public void CalculateWPM_ShouldReturnCorrectWPM()
        {
            // Arrange: Set the time (in milliseconds) and the word count
            long elapsedMilliseconds = 300000; // 5 minutes
            int wordCount = 600; // 600 words

            // Act: Call the CalculateWPM method
            var wpm = _quizService.CalculateWPM(elapsedMilliseconds, wordCount);

            // Assert: Check if the WPM is calculated correctly (600 words / 5 minutes = 120 WPM)
            Assert.Equal(120, wpm);
        }

        [Fact]
        public void CalculateWPM_ShouldReturnZero_WhenElapsedMillisecondsOrWordCountIsZero()
        {
            // Act & Assert: Ensure that the WPM is 0 if either the elapsed time or word count is zero
            Assert.Equal(0, _quizService.CalculateWPM(0, 100));  // 0 milliseconds
            Assert.Equal(0, _quizService.CalculateWPM(1000, 0));  // 0 words
            Assert.Equal(0, _quizService.CalculateWPM(0, 0));  // Both zero
        }
    }
}
