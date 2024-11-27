using Xunit;
using Shared.Models;

namespace Server.Tests
{
    public class ParagraphEntityTests
    {
        [Fact]
        public void ParagraphEntity_ShouldHaveCorrectWordCount()
        {
            // Arrange
            var paragraph = new ParagraphEntity
            {
                ParagraphText = "This is a test paragraph to count words.",
                ParagraphWordCount = 0 // Initially set to 0
            };

            // Act
            paragraph.ParagraphWordCount = paragraph.ParagraphText.Split(' ').Length;

            // Assert
            Assert.Equal(8, paragraph.ParagraphWordCount); 
        }
    }
}
