using Xunit;
using Shared.Models;
using System.Text.Json;

public class EntityTests
{
    [Fact]
    public void AttemptEntity_ValidData_StoresCorrectly()
    {
        // Arrange
        var attempt = new AttemptEntity
        {
            AttemptId = 1,
            UserName = "testUser",
            ReadingTime = 50000,
            Wpm = 250.0,
            ParagraphId = 101,
            Score = 85
        };

        // Assert
        Assert.Equal(1, attempt.AttemptId);
        Assert.Equal("testUser", attempt.UserName);
        Assert.Equal(50000, attempt.ReadingTime);
        Assert.Equal(250.0, attempt.Wpm);
        Assert.Equal(101, attempt.ParagraphId);
        Assert.Equal(85, attempt.Score);
    }

    [Fact]
    public void ParagraphEntity_ValidData_StoresCorrectly()
    {
        // Arrange
        var paragraph = new ParagraphEntity
        {
            ParagraphId = 1,
            ParagraphText = "This is a test paragraph.",
            ParagraphWordCount = 5
        };

        // Assert
        Assert.Equal(1, paragraph.ParagraphId);
        Assert.Equal("This is a test paragraph.", paragraph.ParagraphText);
        Assert.Equal(5, paragraph.ParagraphWordCount);
    }
            [Fact]
        public void QuestionEntity_ShouldCorrectlyStoreOptions()
        {
            // Arrange
            var question = new QuestionEntity
            {
                QuestionId = 1,
                ParagraphId = 100,
                Text = "What is 2 + 2?",
                OptionsJson = JsonSerializer.Serialize(new List<string> { "3", "4", "5" }),
                CorrectAnswer = "4"
            };

            // Act
            var options = JsonSerializer.Deserialize<List<string>>(question.OptionsJson);

            // Assert
            Assert.Equal(new List<string> { "3", "4", "5" }, options);
            Assert.Equal("4", question.CorrectAnswer);  // Ensure correct answer matches stored value
        }
}
