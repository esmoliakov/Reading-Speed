using Shared.Models;
using Xunit;

public class SharedModelsTests
{
    [Fact]
    public void ParagraphWordCount_Constructor_ShouldInitializeProperties()
    {
        // Arrange
        int count = 10;
        string textId = "Paragraph1";

        // Act
        var paragraphWordCount = new ParagraphWordCount(count, textId);

        // Assert
        Assert.Equal(count, paragraphWordCount.Count);
        Assert.Equal(textId, paragraphWordCount.TextId);
    }

    [Fact]
    public void ParagraphWordCount_ToString_ShouldReturnFormattedString()
    {
        // Arrange
        int count = 15;
        string textId = "Paragraph42";
        var paragraphWordCount = new ParagraphWordCount(count, textId);

        // Act
        var result = paragraphWordCount.ToString();

        // Assert
        Assert.Equal("15 words in Paragraph42", result);
    }

    [Fact]
    public void AttemptEntity_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        int id = 1;
        string userName = "TestUser";
        long readingTime = 120000; // 2 minutes in milliseconds
        double wpm = 200.5;
        int paragraphId = 42;
        int score = 85;

        // Act
        var attempt = new AttemptEntity
        {
            Id = id,
            UserName = userName,
            ReadingTime = readingTime,
            Wpm = wpm,
            ParagraphId = paragraphId,
            Score = score
        };

        // Assert
        Assert.Equal(id, attempt.Id);
        Assert.Equal(userName, attempt.UserName);
        Assert.Equal(readingTime, attempt.ReadingTime);
        Assert.Equal(wpm, attempt.Wpm);
        Assert.Equal(paragraphId, attempt.ParagraphId);
        Assert.Equal(score, attempt.Score);
    }
}
