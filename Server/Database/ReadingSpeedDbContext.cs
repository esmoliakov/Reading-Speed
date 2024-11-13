using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Server.Database;

public class ReadingSpeedDbContext : DbContext
{
    public DbSet<Attempt> Attempts { get; set; }
    public DbSet<Paragraph> Paragraphs { get; set; }
    public DbSet<Question> Questions { get; set; }
    
    public ReadingSpeedDbContext(DbContextOptions<ReadingSpeedDbContext> options) : base(options){}
}