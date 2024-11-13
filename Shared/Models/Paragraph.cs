using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class Paragraph
{
    [Key]
    public int ParagraphId { get; set; }
    
    public string? ParagraphText { get; set; }
}