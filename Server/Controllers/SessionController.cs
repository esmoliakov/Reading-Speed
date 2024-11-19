using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers;

public class SessionController : ControllerBase
{
    private readonly SessionService _sessionService;

    public SessionController(SessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost("start-session")]
    public async Task<IActionResult> CreateSession()
    {
        var sessionId = await _sessionService.CreateSessionAsync();
        return Ok(sessionId);
    }

    [HttpGet("get-session-id")]
    public IActionResult GetSessionId()
    {
        var sessionId = _sessionService.GetSessionIdFromCookie();

        if (sessionId.HasValue)
        {
            return Ok(sessionId.Value);
        }

        return NotFound("Session ID not found in cookies.");
    }
}