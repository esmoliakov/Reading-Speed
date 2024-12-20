using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Interfaces;

namespace Shared.Models;

public class QuestionEntity : IEntity
{
    [Key]
    public int Id { get; set; }
    public int ParagraphId { get; set; }
    
    [ForeignKey("ParagraphId")]
    public ParagraphEntity Paragraph { get; set; }
    
    public string Text { get; set; }

    public static implicit operator QuestionEntity(Question v)
    {
        throw new NotImplementedException();
    }
}