using System.ComponentModel.DataAnnotations;

namespace Shared.Models;
public class Question : IComparable 
{
    public int ParagraphId { get; set; }
    public int Id { get; set; }
    public string Text { get; set; }
    public List<string> Options { get; set; }
    public string CorrectAnswer { get; set; }
    
    public int CompareTo(Object obj)
    {
        if (obj == null) return 1;
        
        Question otherQuestion = obj as Question;
        
        if(otherQuestion != null)
            return this.Id.CompareTo(otherQuestion.Id);
        else
            throw new ArgumentException("Object is not a question");
    }
}