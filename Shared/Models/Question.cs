using System.ComponentModel.DataAnnotations;

namespace Shared.Models;
public class Question : IComparable 
{
    [Key]
    public int ParagraphId { get; set; }
    public int id { get; set; }
    public string text { get; set; }
    public List<string> options { get; set; }
    public string correctAnswer { get; set; }
    public string userAnswer { get; set; }
    
    public int CompareTo(Object obj)
    {
        if (obj == null) return 1;
        
        Question otherQuestion = obj as Question;
        
        if(otherQuestion != null)
            return this.text.CompareTo(otherQuestion.text);
        else
            throw new ArgumentException("Object is not a question");
    }
}