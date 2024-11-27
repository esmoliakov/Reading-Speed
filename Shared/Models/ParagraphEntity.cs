using System.ComponentModel.DataAnnotations;
using Shared.Interfaces;

namespace Shared.Models;

public class ParagraphEntity : IEntity
{
    [Key]
    public int Id { get; set; }
    public string ParagraphText { get; set; }
    public int ParagraphWordCount { get; set; }
}