using Database.Entity;

namespace Database;

using Microsoft.EntityFrameworkCore;

public class DatabaseContext(
    DbContextOptions<DatabaseContext> options)
    : DbContext(options)
{
    public const string SchemaName = "champbot";
    
    public DbSet<MessageEntity> Message { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);
        
        MessageEntity.Configure(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
    }
}