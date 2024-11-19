using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class SessionEntity
{
    [Key]
    public int SessionId { get; set; }
    public int ParagraphId { get; set; }
}