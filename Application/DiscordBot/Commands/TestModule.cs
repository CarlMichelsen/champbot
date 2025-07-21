using Discord;
using Discord.Interactions;

namespace Application.DiscordBot.Commands;

public class TestModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("echo", "Echo a message")]
    public async Task EchoAsync(
        [Summary("message", "The message to echo")] string message,
        [Summary("ephemeral", "Only visible to you")] bool ephemeral = false)
        => await this.RespondAsync(message, ephemeral: ephemeral);

    [SlashCommand("userinfo", "Get user information")]
    public async Task UserInfoAsync([Summary("user", "Target user")] IUser? user = null)
    {
        user ??= this.Context.User;
        var embed = new EmbedBuilder()
            .WithTitle($"User Info: {user.Username}")
            .AddField("ID", user.Id, true)
            .AddField("Created", user.CreatedAt.ToString("yyyy-MM-dd"), true)
            .WithColor(Color.Blue)
            .Build();
        
        await this.RespondAsync(embed: embed);
    }
}