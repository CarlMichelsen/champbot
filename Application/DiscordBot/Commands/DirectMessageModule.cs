using Discord;
using Discord.Interactions;

namespace Application.DiscordBot.Commands;

public class DirectMessageModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("dm", "Send a direct message from the bot (the message will reveal who sent it)")]
    public async Task DirectMessageMe(
        [Summary("user", "User to message")] IUser? user,
        [Summary("message", "Message to send")] string message)
    {
        var sender = this.Context.User ?? throw new NullReferenceException("Unable to get sender");
        var targetUser = user ?? sender;
        
        try
        {
            var dmChannel = await targetUser.CreateDMChannelAsync();
            await dmChannel.SendMessageAsync(
                message,
                embed: new EmbedBuilder()
                    .AddField("Was instructed to send this message by", $"<@{sender.Id}>")
                    .Build());
            await this.RespondAsync($"DM sent to {targetUser.Username}", ephemeral: true);
        }
        catch (Discord.Net.HttpException ex) when (ex.DiscordCode == DiscordErrorCode.CannotSendMessageToUser)
        {
            await this.RespondAsync("Cannot send DM to this user (DMs disabled or blocked)", ephemeral: true);
        }
    }
}