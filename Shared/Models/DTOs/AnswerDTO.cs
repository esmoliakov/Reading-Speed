namespace Shared.Models.DTOs;

public class AnswerDTO
{
    public int QuestionId { get; set; }
    public String Answer { get; set; }
    public bool IsCorrectAnswer { get; set; }
}