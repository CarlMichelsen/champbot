import type { DiscordUserDto } from "./discordUserDto";

export type DiscordTextChannelDto = {
    textChannelId: string;
    category?: string;
    channelName: string;
    members: DiscordUserDto[];
}