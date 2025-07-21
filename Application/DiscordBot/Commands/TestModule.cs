using Discord;
using Discord.Interactions;

namespace Application.DiscordBot.Commands;

public class TestModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("userinfo", "Get user information")]
    public async Task UserInfo([Summary("user", "Target user")] IUser? user = null)
    {
        user ??= this.Context.User;
        var embed = new EmbedBuilder()
            .WithTitle($"User Info: {user.Username}")
            .AddField("ID", user.Id, true)
            .AddField("Created", user.CreatedAt.ToString("yyyy-MM-dd"), true)
            .WithColor(Color.Blue)
            .Build();
        
        await this.RespondAsync(embed: embed, ephemeral: true);
    }
}