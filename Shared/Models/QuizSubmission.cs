namespace Shared.Models;

public class QuizSubmission
{
    public List<Question> Questions { get; set; }
    public string UserName { get; set; }
    public string Filename { get; set; }
}