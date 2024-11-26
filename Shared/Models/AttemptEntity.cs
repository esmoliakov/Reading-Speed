using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class AttemptEntity
{
    [Key]
    public int AttemptId { get; set; }
    
    public string UserName { get; set; }
    public int ReadingTime { get; set; }
    public int Wpm { get; set; }
    public int ParagraphId { get; set; }
    public int Score { get; set; }
}