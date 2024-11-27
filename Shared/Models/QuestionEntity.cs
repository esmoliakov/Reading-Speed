using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models;

public class QuestionEntity
{
    [Key]
    public int QuestionId { get; set; }
    public int ParagraphId { get; set; }
    
    [ForeignKey("ParagraphId")]
    public ParagraphEntity Paragraph { get; set; }
    
    public string Text { get; set; }

    // JSON string to store options
    public string OptionsJson { get; set; }

    public string CorrectAnswer { get; set; }
}