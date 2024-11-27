using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Server.Database;

public class ReadingSpeedDbContext : DbContext
{
    public DbSet<ParagraphEntity> Paragraphs { get; set; }
    public DbSet<QuestionEntity> Questions { get; set; }
    public DbSet<AttemptEntity> Attempts { get; set; }
    
    public ReadingSpeedDbContext(DbContextOptions<ReadingSpeedDbContext> options) : base(options){}
}