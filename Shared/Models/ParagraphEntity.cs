using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class ParagraphEntity
{
    [Key]
    public int ParagraphId { get; set; }
    public string ParagraphText { get; set; }
    public int ParagraphWordCount { get; set; }
}