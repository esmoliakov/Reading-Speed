using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Text.Json;
using System.Collections.Generic;
using Server.Controllers;
using Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Server.Database;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace YourProject.Tests
{
    public class QuizServiceIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public QuizServiceIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetQuestions_ReturnsOkStatusCode_WithQuestions()
        {
            // Arrange
            string fileName = "sampleQuestions.json";

            // Act
            var response = await _client.GetAsync($"api/questions/get-questions?fileName={fileName}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var questions = JsonSerializer.Deserialize<List<Question>>(content);

            Assert.NotNull(questions);
            Assert.True(questions.Count > 0);
        }

        [Fact]
        public async Task GetQuestionsSorted_ReturnsSortedQuestions()
        {
            // Arrange
            string fileName = "sampleQuestions.json";

            // Act
            var response = await _client.GetAsync($"api/questions/get-questions-sorted?fileName={fileName}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var questions = JsonSerializer.Deserialize<List<Question>>(content);

            Assert.NotNull(questions);
            Assert.True(questions.Count > 0);
            Assert.True(questions[0].id < questions[1].id);
        }
    }

    public class QuizControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public QuizControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

    public class TimerControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public TimerControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task StartTimer_ReturnsOkStatusCode()
        {
            // Act
            var response = await _client.GetAsync("api/timer/start");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("Stopwatch started.", content);
        }

        [Fact]
        public async Task StopTimer_ReturnsElapsedTime()
        {
            // Arrange
            await _client.GetAsync("api/timer/start");

            // Act
            var response = await _client.GetAsync("api/timer/stop");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.True(long.TryParse(content, out _));
        }
    }

    public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public UserControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task ResetData_ReturnsOk_WhenDataIsReset()
        {
            // Act
            var response = await _client.PostAsync("api/user/reset", null);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Data reset successfully.", result);
        }
    }
}
}