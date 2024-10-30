using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.Models;
using Server.Exceptions;

namespace Server.Controllers;

[ApiController]
[Route("api/quiz")]

public class QuizController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<QuizController> _logger;

    public QuizController(IWebHostEnvironment environment, ILogger<QuizController> logger)
    {
        _environment = environment;
        _logger = logger;
    }
    
    [HttpPost("save-score")]
    public IActionResult SubmitQuiz(QuizSubmission quizSubmission)
    {

        if (quizSubmission.Questions == null || quizSubmission.Questions.Count == 0)
        {
            return NoContent();
        }
        
        try
        {
            int quizScore = QuizService.QuizScore(quizSubmission.Questions);
            var filePath = Path.Combine(_environment.ContentRootPath, "Files", quizSubmission.Filename);
                
            UserDataService.SaveUserRecord(quizSubmission.UserName, quizScore, filePath);
                
            return Ok();
        }
        catch (EmptyNameException ex)
        {
            _logger.LogWarning(ex.Message); // Use LogWarning to output just the exception message
            return BadRequest(ex.Message); // 400 bad request
        }
        catch (Exception ex)
        {
            // log full exception message
            _logger.LogError(ex, "An unexpected error occurred while submitting the quiz."); // Log the error
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}
