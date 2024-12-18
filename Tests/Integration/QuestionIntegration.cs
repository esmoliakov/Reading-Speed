using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Shared.Models;
using Shared.Models.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Server.Database;
using Microsoft.EntityFrameworkCore;

public class QuestionsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public QuestionsControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateQuestion_ReturnsOk_WhenParagraphExists()
    {
        // Arrange
        var paragraph = new ParagraphEntity
        {
            ParagraphText = "Sample paragraph text"
        };

        // Add a paragraph to the database
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ReadingSpeedDbContext>();
            context.Paragraphs.Add(paragraph);
            await context.SaveChangesAsync();
        }

        var paragraphId = paragraph.Id; 

        var createQuestionDTO = new CreateQuestionDTO
        {
            ParagraphId = paragraphId, 
            Text = "What is the speed of light?"
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/questions/create-question", createQuestionDTO);

        // Assert
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            // Log the response content for debugging
            var content = await response.Content.ReadAsStringAsync();
            Assert.True(false, $"Request failed with status code {response.StatusCode} and response: {content}");
        }
        else
        {
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            // Verify that the question was added to the database
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ReadingSpeedDbContext>();
                var question = await context.Questions.FirstOrDefaultAsync(q => q.Text == createQuestionDTO.Text);
                Assert.NotNull(question);
                Assert.Equal(createQuestionDTO.Text, question.Text);
            }
        }
    }

    [Fact]
    public async Task CreateQuestion_ReturnsBadRequest_WhenParagraphDoesNotExist()
    {
        // Arrange
        var createQuestionDTO = new CreateQuestionDTO
        {
            ParagraphId = 999, // non existent
            Text = "What is the speed of light?"
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/questions/create-question", createQuestionDTO);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Paragraph with ID 999 does not exist.", content);
    }
}
