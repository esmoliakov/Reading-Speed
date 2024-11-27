using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Shared.Interfaces;
using Shared.Models;
using Shared.Models.DTOs;

namespace Server.Controllers;

[ApiController]
[Route("api/questions")]
public class QuestionsController : ControllerBase
{
    private readonly ReadingSpeedDbContext _context;
    private readonly IRepository<QuestionEntity> _questionRepository;
    private readonly IRepository<ParagraphEntity> _paragraphRepository;

    public QuestionsController(ReadingSpeedDbContext context)
    {
        _context = context;
        _questionRepository = new Repository<QuestionEntity>(context);
        _paragraphRepository = new Repository<ParagraphEntity>(context);
    }
    
    [HttpPost("create-question")]
    public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionDTO question)
    {
        //Check if the ParagraphId exists
        //var paragraphExists = await _context.Paragraphs.FirstOrDefaultAsync(p => p.Id == question.ParagraphId);
        var paragraphExists = await _paragraphRepository.GetByIdAsync(question.ParagraphId);
        if (paragraphExists == null)
        {
            return BadRequest($"Paragraph with ID {question.ParagraphId} does not exist.");
        }

        QuestionEntity newQuestion = new QuestionEntity();
        newQuestion.ParagraphId = question.ParagraphId;
        newQuestion.Text = question.Text;
        newQuestion.OptionsJson = JsonSerializer.Serialize(question.Options);
        newQuestion.CorrectAnswer = question.CorrectAnswer;
        
        // Automatically increment QuestionId
        var lastQuestion = await _context.Questions.OrderByDescending(q => q.Id).FirstOrDefaultAsync();
        newQuestion.Id = (lastQuestion?.Id ?? 0) + 1;

        _context.Questions.Add(newQuestion);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("get-questions")]
    public async Task<IActionResult> GetQuestions([FromQuery] int paragraphId)
    {
        var questions = await _context.Questions.Where(q => q.ParagraphId == paragraphId).ToListAsync();
        
        if (!questions.Any())
        {
            return NotFound($"No questions found for Paragraph ID {paragraphId}.");
        }
        
        return Ok(questions);
    }

    [HttpDelete("delete-question")]
    public async Task<IActionResult> DeleteQuestion([FromQuery] int questionId)
    {
        var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == questionId);
        
        if (question == null)
            return NotFound($"Question with ID {questionId} was not found.");
        
        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}