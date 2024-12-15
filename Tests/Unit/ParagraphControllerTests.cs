using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.Controllers;
using Server.Database;
using Shared.Models;
using Xunit;

public class ParagraphControllerTests
{
    private readonly ParagraphController _controller;
    private readonly ReadingSpeedDbContext _dbContext;

    public ParagraphControllerTests()
    {
        var options = new DbContextOptionsBuilder<ReadingSpeedDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _dbContext = new ReadingSpeedDbContext(options);

        SeedDatabase();

        _controller = new ParagraphController(_dbContext);
    }

    private void SeedDatabase()
    {
        _dbContext.Paragraphs.AddRange(
            new ParagraphEntity { ParagraphText = "First Paragraph", ParagraphWordCount = 2 },
            new ParagraphEntity { ParagraphText = "Second Paragraph", ParagraphWordCount = 2 }
        );
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task AddParagraph_ShouldReturnOk_WhenParagraphTextIsValid()
    {
        // Arrange
        var paragraphText = "This is a valid paragraph.";

        // Act
        var result = await _controller.AddParagraph(paragraphText);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task AddParagraph_ShouldReturnBadRequest_WhenParagraphTextIsEmpty()
    {
        // Arrange
        var paragraphText = ""; 

        // Act
        var result = await _controller.AddParagraph(paragraphText); 

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetParagraphText_ShouldReturnNotFound_WhenParagraphDoesNotExist()
    {
        // Arrange
        var paragraphId = 999;

        // Act
        var result = await _controller.GetParagraphText(paragraphId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result); 
    }

    [Fact]
    public async Task GetParagraphText_ShouldReturnOk_WhenParagraphExists()
    {
        // Arrange
        var paragraphId = 1; // Existing paragraph ID
        var paragraphText = "Sample paragraph text";
        var paragraph = new ParagraphEntity { Id = paragraphId, ParagraphText = paragraphText, ParagraphWordCount = 3 };

        // Assuming you are using an in-memory database or similar, ensure the paragraph exists
        var options = new DbContextOptionsBuilder<ReadingSpeedDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new ReadingSpeedDbContext(options))
        {
            context.Paragraphs.Add(paragraph);
            await context.SaveChangesAsync();

            var controller = new ParagraphController(context);

            var result = await controller.GetParagraphText(paragraphId); 

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedParagraphText = Assert.IsType<string>(okResult.Value);
            Assert.Equal(paragraphText, returnedParagraphText); // Ensure the paragraph text matches
        }
    }

    [Fact]
    public async Task DeleteParagraph_ShouldReturnNotFound_WhenParagraphDoesNotExist()
    {
        var paragraphId = 999;

        var result = await _controller.DeleteParagraph(paragraphId);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteParagraph_ShouldReturnNoContent_WhenParagraphExists()
    {
        // Arrange
        var paragraphId = 1;

        // Act
        var result = await _controller.DeleteParagraph(paragraphId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateParagraph_ShouldReturnNotFound_WhenParagraphDoesNotExist()
    {
        // Arrange
        var paragraphId = 999;
        var updatedText = "Updated paragraph text.";

        // Act
        var result = await _controller.UpdateParagraph(paragraphId, updatedText);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdateParagraph_ShouldReturnNoContent_WhenParagraphIsUpdatedSuccessfully()
    {
        // Arrange
        var paragraphId = 1;
        var updatedText = "Updated paragraph text.";

        // Act
        var result = await _controller.UpdateParagraph(paragraphId, updatedText);

        // Assert
        Assert.IsType<NoContentResult>(result);

        var paragraph = await _dbContext.Paragraphs.FirstOrDefaultAsync(p => p.Id == paragraphId);
        Assert.NotNull(paragraph);
        Assert.Equal(updatedText, paragraph.ParagraphText);
    }
    [Fact]
    public void GetLastParagraphId_ShouldReturnNotFound_WhenNoParagraphsExist()
    {
        // Arrange
        var emptyContext = new ReadingSpeedDbContext(
            new DbContextOptionsBuilder<ReadingSpeedDbContext>().UseInMemoryDatabase("EmptyDb").Options
        );
        var controller = new ParagraphController(emptyContext);

        // Act
        var result = controller.GetLastParagraphId(); // Returns ActionResult<int>

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);  // Access Result to get the actual ObjectResult
        Assert.Equal("Paragraph not found.", notFoundResult.Value);
    }



}
