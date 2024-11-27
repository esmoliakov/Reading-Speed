using System.Collections.Generic;
using Xunit;
using Shared.Models;
using Shared.Models.DTOs;
using Server.Services;

public class QuizServiceTests
{
    [Fact]
    public void QuizScore_CorrectAnswers_ReturnsCorrectScore()
    {
        // Arrange
        var quizService = new QuizService();

        var userAnswers = new List<UserAnswerDTO>
        {
            new UserAnswerDTO
            {
                Question = new QuestionEntity
                {
                    CorrectAnswer = "Paris"
                },
                UserAnswer = "Paris"
            },
            new UserAnswerDTO
            {
                Question = new QuestionEntity
                {
                    CorrectAnswer = "London"
                },
                UserAnswer = "London"
            },
            new UserAnswerDTO
            {
                Question = new QuestionEntity
                {
                    CorrectAnswer = "Berlin"
                },
                UserAnswer = "Madrid"
            }
        };

        // Act
        var score = quizService.QuizScore(userAnswers);

        // Assert
        Assert.Equal(67, score); 
    }
    [Fact]
    public void QuizScore_AllIncorrectAnswers_Returns0Score()
    {
        // Arrange
        var quizService = new QuizService();
        var userAnswers = new List<UserAnswerDTO>
        {
            new UserAnswerDTO
            {
                Question = new QuestionEntity { CorrectAnswer = "Paris" },
                UserAnswer = "London"
            },
            new UserAnswerDTO
            {
                Question = new QuestionEntity { CorrectAnswer = "London" },
                UserAnswer = "Berlin"
            },
            new UserAnswerDTO
            {
                Question = new QuestionEntity { CorrectAnswer = "Berlin" },
                UserAnswer = "Paris"
            }
        };

        // Act
        var score = quizService.QuizScore(userAnswers);

        // Assert
        Assert.Equal(0, score);  // All answers are incorrect, so the score should be 0%
    }

}
