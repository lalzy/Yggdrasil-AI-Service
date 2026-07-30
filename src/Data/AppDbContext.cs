// AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using Yggdrasil.Models;

namespace Yggdrasil.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var modelTypes = typeof(AppDbContext).Assembly
            .GetTypes()
            .Where(t => t.Namespace == "Yggdrasil.Models" && t.IsClass && !t.IsAbstract);
        
        foreach (var type in modelTypes)
            modelBuilder.Entity(type);

        // Add this:
        modelBuilder.Entity<Character>()
            .HasMany(c => c.Worlds)
            .WithMany(w => w.Characters)
            .UsingEntity(j => j.ToTable("CharacterWorld"));
    }
}