import type { DiscordUserDto } from "./discordUserDto";

export type DiscordChannelDto = {
    guildId: string;
    channelId: string;
    category?: string;
    channelName: string;
    members: DiscordUserDto[];
}