using Xunit;
using System.IO;
using Server.Services;

public class TimerServicesTests
{
    private readonly string TestTimeFilePath;

    public TimerServicesTests()
    {
        // Use a temporary directory for storing the test file
        TestTimeFilePath = Path.Combine(Path.GetTempPath(), "stopwatch.txt");

        // Clean up the file before each test
        if (File.Exists(TestTimeFilePath))
        {
            File.Delete(TestTimeFilePath);
        }
    }


    [Fact]
    public void FindBestReadingTime_ReturnsCorrectBestTime()
    {
        // Arrange
        // Write multiple times into the file
        File.WriteAllText(TestTimeFilePath, "60000\n50000\n70000");

        // Act
        var bestTime = TimerServices.FindBestReadingTime(TestTimeFilePath);

        // Assert
        // Check if the best time is the minimum (50000)
        Assert.Equal("50000", bestTime);
    }

    // Cleanup after tests
    ~TimerServicesTests()
    {
        if (File.Exists(TestTimeFilePath))
        {
            File.Delete(TestTimeFilePath);
        }
    }
}
