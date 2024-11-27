using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Text.Json;
using System.Collections.Generic;
using Server.Controllers;
using Shared.Models;
using System.IO;

public class TimerControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public TimerControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task StartTimer_ReturnsOk_WhenStarted()
    {
        // Act
        var response = await _client.GetAsync("api/timer/start");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Stopwatch started.", content);
    }
    [Fact]
    public async Task StopTimer_ReturnsElapsedTime_WhenStopped()
    {
        // Act
        await _client.GetAsync("api/timer/start"); // Start the timer
        var response = await _client.GetAsync("api/timer/stop"); // Stop the timer

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.True(long.TryParse(content, out _)); // Check if the content is a valid long number representing time
    }

}
