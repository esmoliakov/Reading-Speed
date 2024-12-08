using System;
using System.IO;
using System.Text;
using Xunit;
using Server.Services;

public class FileReaderServiceTests
{
    private const string TestFilePath = "testfile1.txt";
    private const string EmptyTestFilePath = "emptyfile.txt";

    public FileReaderServiceTests()
    {
        // Setup test files for reading
        if (!File.Exists(TestFilePath))
        {
            File.WriteAllText(TestFilePath, "Line 1\nLine 2\nLine 3");
        }

        if (!File.Exists(EmptyTestFilePath))
        {
            File.WriteAllText(EmptyTestFilePath, string.Empty);
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

    [Fact]
    public void ReadTextLastLine_LogsMessageWhenFileIsEmpty()
    {
        // Arrange: Capture the console output.
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        // Act: Read the last line of an empty file
        var lastLine = FileReaderService.ReadTextLastLine(EmptyTestFilePath);

        // Assert: Verify the output and lastLine
        Assert.Null(lastLine);  // Ensure no last line is returned
        Assert.Contains("File is empty.", stringWriter.ToString());  // Ensure the message is logged
    }
}
