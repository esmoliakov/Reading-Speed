namespace Shared.Models.DTOs;

public class UserAnswerDTO
{
    public Question Question { get; set; }
    public String UserAnswer { get; set; }

    public UserAnswerDTO()
    {
        Question = new Question();
        UserAnswer = "";
    }
}