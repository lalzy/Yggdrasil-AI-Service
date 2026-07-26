// DatabaseFixture.cs

using Yggdrasil.Models;
using Yggdrasil.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class DatabaseFixture : IDisposable
{
    public SqliteConnection Connection { get; }
    public AppDbContext Db { get; }

    public DatabaseFixture()
    {
        Connection = new SqliteConnection("Data Source=:memory:");
        Connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(Connection)
            .Options;

        Db = new AppDbContext(options);
        Db.Database.EnsureCreated();

        // Seed global data
        Db.Set<Settings>().Add(new Settings());
        Db.SaveChanges();
    }

    public void Dispose()
    {
        Db.Dispose();
        Connection.Dispose();
    }
}
