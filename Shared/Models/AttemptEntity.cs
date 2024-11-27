using System.ComponentModel.DataAnnotations;
using Shared.Interfaces;

namespace Shared.Models;

public class AttemptEntity : IEntity
{
    [Key]
    public int Id { get; set; }
    
    public string UserName { get; set; }
    public long ReadingTime { get; set; }
    public double Wpm { get; set; }
    public int ParagraphId { get; set; }
    public int Score { get; set; }
}