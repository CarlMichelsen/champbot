import type { DiscordChannelDto } from "./discordChannelDto";
import type { DiscordUserDto } from "./discordUserDto";

export type DiscordGuildDto = {
    guildId: string;
    guildCreatorId: string;
    guildName: string;
    guildIconUrl?: string;
    members: DiscordUserDto[];
    voiceChannels: DiscordChannelDto[];
    textChannels: DiscordChannelDto[];
}