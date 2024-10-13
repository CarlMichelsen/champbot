namespace Database.Entity;

public class BirthdayEntity
{
    public long Id { get; init; }

    public required string Name { get; init; }

    public required int Day { get; init; }

    public required int Month { get; init; }

    public string? DiscordUserId { get; set; }
}