using Discord;

namespace Presentation.DiscordBot;

public interface IMessageDeletionHandler
{
    Task HandleMessageDeletion(Cacheable<IMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel);
}