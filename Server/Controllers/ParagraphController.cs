using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Shared.Interfaces;
using Shared.Models;

namespace Server.Controllers;

[ApiController]
[Route("api/paragraph")]
public class ParagraphController : ControllerBase
{
    private readonly ReadingSpeedDbContext _context;
    private readonly IRepository<ParagraphEntity> _paragraphRepository;


    public ParagraphController(ReadingSpeedDbContext context)
    {
        _context = context;
        _paragraphRepository = new Repository<ParagraphEntity>(context);

    }

    [HttpPost("add-paragraph")]
    public async Task<IActionResult> AddParagraph([FromQuery] string paragraphText)
    {
        if (string.IsNullOrWhiteSpace(paragraphText))
            return BadRequest("Paragraph text cannot be empty");

        // Get the current highest ParagraphId
        var newParagraphId = 1; // Default value if the table is empty

        var maxId = await _context.Paragraphs
            .OrderByDescending(p => p.Id)
            .Select(p => p.Id)
            .FirstOrDefaultAsync();

        if (maxId != 0) newParagraphId = maxId + 1;

       var calculatedWordCount = paragraphText.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
    .Length;


        // Create a new paragraph instance
        var newParagraph = new ParagraphEntity
        {
            Id = newParagraphId,
            ParagraphText = paragraphText,
            ParagraphWordCount = calculatedWordCount
        };
            
        await _paragraphRepository.AddAsync(newParagraph);

        return Ok();
    }

    [HttpGet("get-paragraph-text")]
    public async Task<IActionResult> GetParagraphText([FromQuery] int paragraphId)
    {
        var paragraph = await _paragraphRepository.GetByIdAsync(paragraphId);
        
        if (paragraph == null) return NotFound("Paragraph not found.");

        return Ok(paragraph.ParagraphText);
    }

    [HttpGet("get-last-id")]
    public ActionResult<int> GetLastParagraphId()
    {
        var paragraph = _context.Paragraphs.OrderByDescending(p => p.Id).FirstOrDefault();
        if (paragraph == null) return NotFound("Paragraph not found.");
        return Ok(paragraph.Id);
    }

    [HttpPut("update-paragrapgh")]
    public async Task<IActionResult> UpdateParagraph([FromQuery] int paragraphId, [FromQuery] string paragraphText)
    {
        var existingParagraph = await _paragraphRepository.GetByIdAsync(paragraphId);
         
        if (existingParagraph == null) return NotFound("Paragraph not found.");

        var calculatedWordCount = paragraphText.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Length;

        
        // Update the properties of the existing paragraph
        existingParagraph.ParagraphText = paragraphText;
        existingParagraph.ParagraphWordCount = calculatedWordCount;

        await _context.SaveChangesAsync();

        return NoContent(); // Return 204 No Content to indicate a successful update
    }

    [HttpDelete("delete-paragraph")]
    public async Task<IActionResult> DeleteParagraph([FromQuery] int paragraphId)
    {
        var paragraph = await _paragraphRepository.GetByIdAsync(paragraphId);

        if (paragraph == null) return NotFound("Paragraph not found.");
        
        await _paragraphRepository.DeleteAsync(paragraph);

        return NoContent(); // Indicates the operation was successful with no response body
    }
        private const string FontSizeKey = "FontSize";

        [HttpPost("set-font-size")]
        public IActionResult SetFontSize([FromBody] string fontSize)
        {
            HttpContext.Session.SetString(FontSizeKey, fontSize);
            return Ok();
        }

        [HttpGet("get-font-size")]
        public IActionResult GetFontSize()
        {
            var fontSize = HttpContext.Session.GetString(FontSizeKey) ?? "16px"; // Numatytas dydis
            return Ok(fontSize);
        }
}