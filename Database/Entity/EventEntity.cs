using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Entity;

public class EventEntity
{
    public long Id { get; init; }

    public required string EventName { get; set; }

    public required DateTime EventTimeUtc { get; set; }

    public required List<ReminderEntity> Reminders { get; init; }

    public required DateTime CreatedUtc { get; init; }

    public long UserId { get; set; }

    public static void OnModelCreating(EntityTypeBuilder<EventEntity> entity)
    {
    }
}