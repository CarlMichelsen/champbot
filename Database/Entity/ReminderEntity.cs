using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Entity;

public class ReminderEntity
{
    public long Id { get; init; }

    public string? ReminderNote { get; set; }

    public required long EventId { get; init; }

    public required EventEntity Event { get; init; }

    public required TimeSpan TimeBeforeEvent { get; set; }

    public required bool Reminded { get; init; }

    public required DateTime CreatedUtc { get; init; }

    public static void OnModelCreating(EntityTypeBuilder<ReminderEntity> entity)
    {
        entity
            .HasOne(r => r.Event)
            .WithMany(e => e.Reminders)
            .HasForeignKey(r => r.EventId);
    }
}