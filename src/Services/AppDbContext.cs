using Microsoft.EntityFrameworkCore;
using Yggdrasil.Models;

namespace Yggdrasil.Services;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ChatLogs> ChatLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var modelTypes = typeof(AppDbContext).Assembly
            .GetTypes()
            .Where(t => t.Namespace == "Yggdrasil.Models" && t.IsClass && !t.IsAbstract);

        foreach (var type in modelTypes)
            modelBuilder.Entity(type);
    }
}