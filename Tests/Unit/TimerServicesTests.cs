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

    // Test for WriteTimeToFile method
    [Fact]
    public void WriteTimeToFile_AppendsTimeToFile_WhenFileExists()
    {
        // Arrange
        // Create the file first, simulating that it already exists
        File.WriteAllText(TestTimeFilePath, "1000\n");

        // Act
        // Call the method to append a new time to the file
        TimerServices.WriteTimeToFile(2000, TestTimeFilePath);

        // Assert
        var fileContents = File.ReadAllLines(TestTimeFilePath);
        
        // Assert that the file contains two lines with the expected times
        Assert.Equal(2, fileContents.Length);
        Assert.Equal("1000", fileContents[0]); // Initial content
        Assert.Equal("2000", fileContents[1]); // Appended time
    }

    // Test for FindBestReadingTime method
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
