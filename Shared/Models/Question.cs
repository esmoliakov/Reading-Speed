namespace Shared.Models;
public class Question
{
    public int id { get; set; }
    public string text { get; set; }
    public List<string> options { get; set; }
    public string correctAnswer { get; set; }
    public string userAnswer { get; set; }
}