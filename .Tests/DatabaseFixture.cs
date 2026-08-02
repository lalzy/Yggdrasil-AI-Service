// DatabaseFixture.cs

using Yggdrasil.Models;
using Yggdrasil.Data;

public class DatabaseFixture : IDisposable
{
    public SqliteConnection Connection { get; }
    public DbContextOptions<AppDbContext> Options { get; }

    protected readonly DatabaseFixture _fixture;
    
    public DatabaseFixture(){
        Connection = new SqliteConnection("Data Source=:memory:");
        Connection.Open();

        Options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(Connection)
            .Options;

        using var db = new AppDbContext(Options);
        db.Database.EnsureCreated();
        db.Set<Settings>().Add(new Settings());
        db.SaveChanges();
    }

    public void Reset(){
        using var db = CreateContext();
        foreach (var entity in db.Model.GetEntityTypes()){
            if (entity.ClrType == typeof(Settings)) continue;
            var dbSet = db.Model.FindEntityType(entity.ClrType);
            db.Database.ExecuteSqlRaw($"DELETE FROM \"{entity.GetTableName()}\"");
        }
    }
    
    public AppDbContext CreateContext() => new AppDbContext(Options);

    public void Dispose() => Connection.Dispose();
}
