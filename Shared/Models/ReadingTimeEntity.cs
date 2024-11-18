using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models;

public class ReadingTimeEntity
{
    [Key]
    public int SessionId { get; set; }

    [ForeignKey("SessionId")]
    public SessionEntity Session { get; set; }

    public TimeSpan ReadingTimeValue { get; set; }
}