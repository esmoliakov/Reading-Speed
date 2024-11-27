using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Shared.Models;

namespace Server.Controllers;

[ApiController]
[Route("api/paragraph")]
public class ParagraphController : ControllerBase
{
    private readonly ReadingSpeedDbContext _context;

    public ParagraphController(ReadingSpeedDbContext context)
    {
        _context = context;
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

        var calculatedWordCount = paragraphText.Split([' ', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries)
            .Length;

        // Create a new paragraph instance
        var newParagraph = new ParagraphEntity
        {
            Id = newParagraphId,
            ParagraphText = paragraphText,
            ParagraphWordCount = calculatedWordCount
        };

        // Add the new paragraph to the database
        _context.Paragraphs.Add(newParagraph);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("get-paragraph-text")]
    public async Task<IActionResult> GetParagraphText([FromQuery] int paragraphId)
    {
        var paragraph = await _context.Paragraphs
            .FirstOrDefaultAsync(p => p.Id == paragraphId);

        if (paragraph == null) return NotFound("Paragraph not found.");

        return Ok(paragraph.ParagraphText);
    }

    [HttpPut("update-paragrapgh")]
    public async Task<IActionResult> UpdateParagraph([FromQuery] int paragraphId, [FromQuery] string paragraphText)
    {
        var existingParagraph = await _context.Paragraphs
            .FirstOrDefaultAsync(p => p.Id == paragraphId);
        
        if (existingParagraph == null) return NotFound("Paragraph not found.");

        var calculatedWordCount = paragraphText.Split([' ', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries)
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
        // Find the paragraph by ID
        var paragraph = await _context.Paragraphs
            .FirstOrDefaultAsync(p => p.Id == paragraphId);

        if (paragraph == null) return NotFound("Paragraph not found.");

        // Remove the paragraph
        _context.Paragraphs.Remove(paragraph);

        // Save changes to commit the deletion to the database
        await _context.SaveChangesAsync();

        return NoContent(); // Indicates the operation was successful with no response body
    }
}