using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Shared.Interfaces;
using Shared.Models;
using Shared.Models.DTOs;

namespace Server.Controllers;

[ApiController]
[Route("api/answers")]
public class AnswerController : ControllerBase
{
    private readonly ReadingSpeedDbContext _context;
    private readonly IRepository<QuestionEntity> _questionRepository;
    private readonly IRepository<AnswerEntity> _answerRepository;

    public AnswerController(ReadingSpeedDbContext context)
    {
        _context = context;
        _questionRepository = new Repository<QuestionEntity>(context);
        _answerRepository = new Repository<AnswerEntity>(context);
    }

    [HttpPost("create-answer")]
    public async Task<ActionResult<AnswerEntity>> CreateAnswer([FromBody] AnswerDTO answer)
    {
        var questionExists = await _questionRepository.GetByIdAsync(answer.QuestionId);
        if(questionExists == null)
            return BadRequest("Question not found");
        
        AnswerEntity newAnswer = new AnswerEntity();
        newAnswer.QuestionId = answer.QuestionId;
        newAnswer.Answer = answer.Answer;
        newAnswer.IsCorrectAnswer = answer.IsCorrectAnswer;
        
        var lastAnswer = await _context.Answers.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
        newAnswer.Id = (lastAnswer?.Id ?? 0) + 1;
        
        await _answerRepository.AddAsync(newAnswer);

        return Ok();
    }
    
    [HttpPost("create-answers")]
    public async Task<ActionResult<AnswerEntity>> CreateAnswer([FromBody] List<AnswerDTO> answers)
    {
        foreach (var answer in answers)
        {
            var questionExists = await _questionRepository.GetByIdAsync(answer.QuestionId);
            if (questionExists == null)
                return BadRequest("Question not found");

            AnswerEntity newAnswer = new AnswerEntity();
            newAnswer.QuestionId = answer.QuestionId;
            newAnswer.Answer = answer.Answer;
            newAnswer.IsCorrectAnswer = answer.IsCorrectAnswer;

            var lastAnswer = await _context.Answers.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
            newAnswer.Id = (lastAnswer?.Id ?? 0) + 1;

            await _answerRepository.AddAsync(newAnswer);
        }

        return Ok();
    }
    
    [HttpGet("get-answers")]
    public async Task<IActionResult> GetAnswers([FromQuery] int questionId)
    {
        var answers = await _context.Answers.Where(a => a.QuestionId == questionId).ToListAsync();
        
        if (!answers.Any())
        {
            return NotFound("No answers found");
        }
        
        List<String> answerTexts = new List<String>();
        foreach (var answer in answers)
            answerTexts.Add(answer.Answer);
        
        return Ok(answerTexts);
    }

    [HttpGet("get-correct-answers")]
    public async Task<IActionResult> GetCorrectAnswer([FromQuery] int questionId)
    {
        var answers = await _context.Answers.Where(a => a.QuestionId == questionId && a.IsCorrectAnswer).ToListAsync();
        
        if(answers == null)
            return NotFound("Answer not found");
        
        List<String> answerTexts = new List<String>();
        foreach (var answer in answers)
            answerTexts.Add(answer.Answer);
        
        return Ok(answerTexts);
    }

    [HttpDelete("delete-answer")]
    public async Task<ActionResult<AnswerEntity>> DeleteAnswer([FromQuery] int answerId)
    {
        var answer = await _answerRepository.GetByIdAsync(answerId);
        
        if(answer == null)
            return NotFound("Answer not found");
        
        await _answerRepository.DeleteAsync(answer);

        return NoContent();
    }
}