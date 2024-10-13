using Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace Database;

/// <summary>
/// EntityFramework application context.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DatabaseContext"/> class.
/// </remarks>
/// <param name="options">Options for data-context.</param>
public class DatabaseContext(
    DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    private const string SchemaName = "champbot";
    
    public DbSet<BirthdayEntity> Birthday { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(SchemaName);
    }
}