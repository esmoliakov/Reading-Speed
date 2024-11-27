using System.IO;
using Xunit;
using Server.Services;

public class FileReaderServiceTests
{
    private const string TestFilePath = "testfile.txt";

    public FileReaderServiceTests()
    {
        // Setup a test file for reading
        if (!File.Exists(TestFilePath))
        {
            File.WriteAllText(TestFilePath, "Line 1\nLine 2\nLine 3");
        }
    }

    [Fact]
    public void ReadTextFileWhole_ReturnsCorrectContent()
    {
        // Act
        var content = FileReaderService.ReadTextFileWhole(TestFilePath);

        // Assert
        Assert.Equal("Line 1\nLine 2\nLine 3", content);
    }

    [Fact]
    public void ReadTextLastLine_ReturnsLastLine()
    {
        // Act
        var lastLine = FileReaderService.ReadTextLastLine(TestFilePath);

        // Assert
        Assert.Equal("Line 3", lastLine);
    }
}
