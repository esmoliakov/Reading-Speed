using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.Models;

namespace Server.Controllers;

[ApiController]
[Route("api/quiz")]

public class QuizController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public QuizController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    
    [HttpPost("save-score")]
    public IActionResult SubmitQuiz(QuizSubmission quizSubmission)
    {
        
        if (quizSubmission.Questions == null || quizSubmission.Questions.Count == 0)
        {
            return NoContent();
        }
        
        int quizScore = QuizService.QuizScore(quizSubmission.Questions);
        
        var filePath = Path.Combine(_environment.ContentRootPath, "Files", quizSubmission.Filename);
        
        UserDataService.SaveUserRecord(quizSubmission.UserName, quizScore, filePath);
        
        return Ok();
    }
}