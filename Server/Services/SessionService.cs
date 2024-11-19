using Microsoft.EntityFrameworkCore;
using Server.Database;
using Shared.Models;

namespace Server.Services;

public class SessionService
{
    private readonly ReadingSpeedDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionService(ReadingSpeedDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> CreateSessionAsync()
    {
        // Generate ParagraphId from existing paragraphs
        int paragraphId = await GenerateParagraphIdAsync();
        
        // Generate a new session ID
        var sessionEntity = new SessionEntity
        {
            ParagraphId = paragraphId
        };

        // Save to database
        _context.Sessions.Add(sessionEntity);
        await _context.SaveChangesAsync();

        // Store session ID in a cookie
        _httpContextAccessor.HttpContext.Response.Cookies.Append("SessionId", sessionEntity.SessionId.ToString(), new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTimeOffset.UtcNow.AddHours(1) // Adjust expiration as needed
        });

        return sessionEntity.SessionId;
    }
    
    private async Task<int> GenerateParagraphIdAsync()
    {
        // Fetch all Paragraph IDs from the database
        var paragraphIds = await _context.Paragraphs
            .Select(p => p.ParagraphId)
            .ToListAsync();

        if (!paragraphIds.Any())
        {
            throw new InvalidOperationException("No paragraphs found in the database.");
        }

        // Randomly select a ParagraphId
        var random = new Random();
        return paragraphIds[random.Next(paragraphIds.Count)];
    }

    public int? GetSessionIdFromCookie()
    {
        var cookie = _httpContextAccessor.HttpContext.Request.Cookies["SessionId"];
        return cookie != null ? int.Parse(cookie) : (int?)null;
    }
}