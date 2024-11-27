using System.IO;
using Xunit;
using Server.Services;
using Shared.Models;
using Server.Exceptions;

public class UserDataServiceTests
{
    private const string TestFilePath = "userRecord.json";

    public UserDataServiceTests()
    {
        // Clean up the file before each test
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }
    }

    [Fact]
    public void SaveUserRecord_ValidData_SavesCorrectRecord()
    {
        // Arrange
        string username = "testUser";
        int quizScore = 85;

        // Act
        UserDataService.SaveUserRecord(username, quizScore, TestFilePath);

        // Assert
        Assert.True(File.Exists(TestFilePath));

        var jsonContent = File.ReadAllText(TestFilePath);
        var userRecord = System.Text.Json.JsonSerializer.Deserialize<AttemptRecord>(jsonContent);

        Assert.Equal(username, userRecord.Name);
        Assert.Equal(quizScore, userRecord.QuizScore);
    }

    [Fact]
    public void SaveUserRecord_EmptyUsername_ThrowsException()
    {
        // Arrange
        string username = string.Empty;
        int quizScore = 85;

        // Act & Assert
        var exception = Assert.Throws<EmptyNameException>(() => UserDataService.SaveUserRecord(username, quizScore, TestFilePath));
        Assert.Equal("Username cannot be null or empty.", exception.Message);
    }

}
