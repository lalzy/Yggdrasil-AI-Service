using Microsoft.EntityFrameworkCore;
using Yggdrasil.Models;

namespace Yggdrasil.Services;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ChatLogs> ChatLogs {get; set;}
}