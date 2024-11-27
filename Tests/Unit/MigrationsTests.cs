using System;
using System.IO;
using Server.Services;
using Xunit;

public class FileWriterServiceTests
{
    [Fact]
    public void WriteToFile_ShouldWriteContentToFile()
    {
        // Arrange
        var tempFilePath = Path.GetTempFileName();
        var expectedContent = "This is a test.";

        try
        {
            // Act
            FileWriterService.WriteToFile(tempFilePath, expectedContent);

            // Assert
            var actualContent = File.ReadAllText(tempFilePath);
            Assert.Equal(expectedContent, actualContent);
        }
        finally
        {
            // Clean up
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    [Fact]
    public void WriteToFile_ShouldOverwriteExistingFileContent()
    {
        // Arrange
        var tempFilePath = Path.GetTempFileName();
        File.WriteAllText(tempFilePath, "Old content");
        var newContent = "New content";

        try
        {
            // Act
            FileWriterService.WriteToFile(tempFilePath, newContent);

            // Assert
            var actualContent = File.ReadAllText(tempFilePath);
            Assert.Equal(newContent, actualContent);
        }
        finally
        {
            // Clean up
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

}
