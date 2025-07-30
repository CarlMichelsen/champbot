using Database.Entity.Id;
using Database.Util;
using Microsoft.EntityFrameworkCore;

namespace Database.Entity;

public class MessageEntity : IEntity
{
    public required MessageEntityId Id { get; init; }
    
    public required ulong MessageDiscordId { get; init; }
    
    public required ulong SenderDiscordId { get; init; }
    
    public required ulong? GuildDiscordId { get; init; }
    
    public required string Source { get; init; }
    
    public required string Content { get; init; }
    
    public required DateTime SentAt { get; init; }
    
    public DateTime? DeletedAt { get; set; }
    
    public static void Configure(ModelBuilder modelBuilder)
    {
        var entityBuilder = modelBuilder
            .Entity<MessageEntity>();

        entityBuilder
            .HasKey(x => x.Id);
        
        entityBuilder
            .Property(x => x.Id)
            .RegisterTypedKeyConversion<MessageEntity, MessageEntityId>(x =>
                new MessageEntityId(x, true));
    }
}