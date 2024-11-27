namespace Shared.Models.DTOs;

public class CreateQuestionDTO
{
    public int ParagraphId { get; set; }
    public string Text { get; set; }
    public List<string> Options { get; set; }
    public string CorrectAnswer { get; set; }
}