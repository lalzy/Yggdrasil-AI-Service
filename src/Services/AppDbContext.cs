using Microsoft.EntityFrameworkCore;
using AIService.Models;

namespace AIService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ChatLogs> ChatLogs {get; set;}
}