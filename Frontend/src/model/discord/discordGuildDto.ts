import type { DiscordTextChannelDto } from "./discordTextChannelDto";
import type { DiscordUserDto } from "./discordUserDto";

export type DiscordGuildDto = {
    guildId: string;
    guildCreatorId: string;
    guildName: string;
    guildIconUrl?: string;
    members: DiscordUserDto[];
    textChannels: DiscordTextChannelDto[];
}