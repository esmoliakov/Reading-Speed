using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Services;

namespace Server.Controllers;

public class SessionController : ControllerBase
{
    private readonly SessionService _sessionService;
    private readonly ReadingSpeedDbContext _context;

    public SessionController(SessionService sessionService, ReadingSpeedDbContext context)
    {
        _sessionService = sessionService;
        _context = context;
    }

    [HttpPost("start-session")]
    public async Task<IActionResult> CreateSession()
    {
        var sessionId = await _sessionService.CreateSessionAsync(_context);
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

    [HttpDelete("delete-session")]
    public async Task<IActionResult> DeleteSession([FromQuery] int sessionId)
    {
        var session = await _context.Sessions.FirstOrDefaultAsync(s => s.SessionId == sessionId);
        
        if (session == null)
            return NotFound("Session ID not found");
        
        _context.Sessions.Remove(session);
        await _context.SaveChangesAsync();
         
        return NoContent();
    }
}