using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Services;
using Shared.Interfaces;
using Shared.Models;
using Shared.Models.DTOs;

namespace Server.Controllers;

[ApiController]
[Route("api/attempt")]
public class AttemptController : ControllerBase
{
    private readonly ReadingSpeedDbContext _context;
    private readonly QuizService _quizService;
    private readonly IRepository<AttemptEntity> _attemptRepository;
    private readonly IRepository<ParagraphEntity> _paragraphRepository;


    public AttemptController(ReadingSpeedDbContext context, QuizService quizService)
    {
        _context = context;
        _quizService = quizService;
        _attemptRepository = new Repository<AttemptEntity>(context);
        _paragraphRepository = new Repository<ParagraphEntity>(context);

    }
    
    [HttpPost("add-attempt")]
    public async Task<IActionResult> AddAttempt([FromBody] CreateAttemptDTO createAttempt)
    {
        var paragraph = await _paragraphRepository.GetByIdAsync(createAttempt.ParagraphId);
        if (paragraph == null)
            return BadRequest($"Paragraph with id {createAttempt.ParagraphId} does not exist");

        AttemptEntity newAttempt = new AttemptEntity();
        newAttempt.UserName = createAttempt.UserName;
        newAttempt.ReadingTime = createAttempt.ReadingTime;
        newAttempt.ParagraphId = createAttempt.ParagraphId;
        
        var lastAttempt = await _context.Attempts.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
        newAttempt.Id = (lastAttempt?.Id ?? 0) + 1;

        newAttempt.Score = _quizService.QuizScore(createAttempt.UserAnswers);
        newAttempt.Wpm = _quizService.CalculateWPM(newAttempt.ReadingTime, paragraph.ParagraphWordCount);
        await _attemptRepository.AddAsync(newAttempt);
        
        return Ok(newAttempt.Id);
    }

    [HttpGet("get-attempt")]
    public async Task<IActionResult> GetAttempt([FromQuery] int attemptId)
    {
        var attempt = await _attemptRepository.GetByIdAsync(attemptId);
        
        if (attempt == null)
            return NotFound($"Attempt with id {attemptId} does not exist");
        
        return Ok(attempt);
    }

    [HttpDelete("delete-attempt")]
    public async Task<IActionResult> DeleteAttempt([FromQuery] int attemptId)
    {
        var attempt = await _attemptRepository.GetByIdAsync(attemptId);
        if (attempt == null)
            return NotFound($"Attempt with id {attemptId} does not exist");
        
        await _attemptRepository.DeleteAsync(attempt);
        
        return NoContent(); 
    }
     [HttpGet("get-best-attempts")]
    public async Task<IActionResult> GetBestAttempts()
    {
        var filteredAttempts = await _context.Attempts
            .Where(a => a.Score >= 75)
            .ToListAsync();
        
        if(filteredAttempts.Count == 0)
            return NotFound("No attempts found");
        
        var attempts = filteredAttempts
            .GroupBy(a => a.UserName)
            .Select(g => g.OrderByDescending(a => a.Wpm).First())
            .OrderByDescending(a => a.Wpm)
            .Take(5)
            .ToList(); 
        
        if(attempts.Count == 0)
            return NotFound("No attempts found");
        
        return Ok(attempts);
    }
}