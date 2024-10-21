using Domain.Dto.Discord;

namespace Interface.Hubs;

public interface IVoiceClientMethods
{
    Task Guilds(List<DiscordGuildDto> guilds);

    Task ConnectedVoiceChannels(List<DiscordChannelDto> channels);
}