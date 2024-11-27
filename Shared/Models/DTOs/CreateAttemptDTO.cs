namespace Shared.Models.DTOs;

public class CreateAttemptDTO
{
    public String UserName { get; set; }
    public int ParagraphId { get; set; }
    public long ReadingTime { get; set; }
}