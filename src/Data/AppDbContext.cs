// AppDbContext.cs

using Yggdrasil.Models;

namespace Yggdrasil.Data;

public class AppDbContext : DbContext{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    ///<summary>Register all Models as DB Tables automatically</summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder){
        var modelTypes = typeof(AppDbContext).Assembly
            .GetTypes()
            .Where(t => t.Namespace == "Yggdrasil.Models" && t.IsClass && !t.IsAbstract);
        
        foreach (var type in modelTypes)
            modelBuilder.Entity(type);

        CreateManyToManyReferences(modelBuilder);
    }

    ///<summary>Setup many to many references for tables</summary>
    private  void CreateManyToManyReferences(ModelBuilder modelBuilder){
        modelBuilder.Entity<Character>()
            .HasMany(c => c.Worlds)
            .WithMany(w => w.Characters)
            .UsingEntity(j => j.ToTable("CharacterWorld"));
    }
}
