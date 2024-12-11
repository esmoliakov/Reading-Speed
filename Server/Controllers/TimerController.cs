using Microsoft.AspNetCore.Mvc;
using Server.Services;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Database;

[ApiController]
[Route("api/timer")]
public class TimerController : ControllerBase
{
    private static Stopwatch stopwatch = new Stopwatch(); 
    private readonly ReadingSpeedDbContext _context;

    public TimerController(ReadingSpeedDbContext context)
    {
        _context = context;
    }

    [HttpGet("start")]
    public IActionResult StartTimer()
    {
        stopwatch.Start();
        return Ok("Stopwatch started.");
    }

    [HttpGet("stop")]
    public async Task<IActionResult> StopTimer()
    {
        if (stopwatch.IsRunning)
        {
            stopwatch.Stop();
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset(); // Reset for next use
            
            return Ok(elapsedMilliseconds);
        }
        return BadRequest("Stopwatch is not running.");
    }

        [HttpGet("get-best-time")]
        public async Task<IActionResult> FindBestTime([FromQuery] string userName)
        {
            var attempt = await _context.Attempts.OrderBy(a => a.ReadingTime).FirstOrDefaultAsync(a => a.UserName == userName);
            if (attempt == null)
                return BadRequest($"User {userName} does not exist.");
            
            return Ok(attempt.ReadingTime);
            
        }
}
