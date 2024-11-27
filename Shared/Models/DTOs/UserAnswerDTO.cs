namespace Shared.Models.DTOs;

public class UserAnswerDTO
{
    public QuestionEntity Question { get; set; }
    public String UserAnswer { get; set; }
}