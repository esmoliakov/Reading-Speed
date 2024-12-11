using System.ComponentModel.DataAnnotations;
using Shared.Interfaces;

namespace Shared.Models;

public class AnswerEntity : IEntity
{
    [Key]
    public int Id { get; set; }
    
    public int QuestionId { get; set; }
    public string Answer { get; set; }
    public bool IsCorrectAnswer { get; set; }
}