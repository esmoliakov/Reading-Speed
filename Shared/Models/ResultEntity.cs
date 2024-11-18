using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models;

public class ResultEntity
{
    [Key]
    public int SessionId { get; set; }

    [ForeignKey("SessionId")]
    public SessionEntity Session { get; set; }
    
    public string Username { get; set; }
    public int Wpm { get; set; }
    public int Score { get; set; }
}