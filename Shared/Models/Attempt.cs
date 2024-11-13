using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class Attempt
{
    [Key]
    public string UserName { get; set; }
    
    public int ReadingTime { get; set; }
    public int Wpm { get; set; }
    public int ParagraphId { get; set; }
    public int Score { get; set; }
}