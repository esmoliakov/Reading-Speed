using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class SessionEntity
{
    [Key]
    public int SeesionId { get; set; }
    public int ParagraphId { get; set; }
}