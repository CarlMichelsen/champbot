import type { DiscordGuildDto } from "../../model/discord/discordGuildDto";
import type { SendDiscordMessageDto } from "../../model/discord/sendDiscordMessageDto";
import { BaseClient } from "../baseClient";
import { hostUrl } from "../endpoints";

export class DiscordClient extends BaseClient
{
    private readonly discordPath = "api/v1/discord";

    protected host: string = hostUrl();
    
    public async getGuilds() {
        return await this.request<DiscordGuildDto[]>("GET", this.discordPath);
    }

    public async sendMessage(sendMessage: SendDiscordMessageDto) {
        return await this.request<DiscordGuildDto[]>("POST", this.discordPath, sendMessage);
    }
}