using System;
using System.Collections.Generic;
using Xunit;
using Shared.Models;
using Shared.Models.DTOs;
using Server.Services;

public class QuizServiceTests
{
    // Test for QuizScore with correct answers
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
        Assert.Equal(67, score); // Expecting 67% score
    }

    // Test for QuizScore with all incorrect answers
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

    // Test for CalculateWPM method
    [Fact]
    public void CalculateWPM_ShouldReturnCorrectWPM()
    {
        // Arrange
        var quizService = new QuizService();
        long elapsedMilliseconds = 120000; // 2 minutes (120,000 ms)
        int wordCount = 240; // Assume 240 words typed

        // Act
        var wpm = quizService.CalculateWPM(elapsedMilliseconds, wordCount);

        // Assert
        Assert.Equal(120, wpm); // 240 words / 2 minutes = 120 WPM
    }

    [Fact]
    public void CalculateWPM_ShouldReturnZero_WhenElapsedTimeIsZero()
    {
        // Arrange
        var quizService = new QuizService();
        long elapsedMilliseconds = 0; // No time passed
        int wordCount = 240;

        // Act
        var wpm = quizService.CalculateWPM(elapsedMilliseconds, wordCount);

        // Assert
        Assert.Equal(0, wpm); // Should return 0 because of zero time
    }

}
