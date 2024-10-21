import { HubConnectionBuilder } from "@microsoft/signalr";
import { getContext, setContext } from "svelte";
import { hostUrl } from "../util/endpoints";
import type { DiscordGuildDto } from "../model/discord/discordGuildDto";
import type { DiscordChannelDto } from "../model/discord/discordChannelDto";

export type ConnectedState = "pending"|"connected"|"disconnected";

export type VoiceStoreState = {
    readonly state: ConnectedState;
    readonly guilds: DiscordGuildDto[];
    readonly voiceChannels: DiscordChannelDto[];
    connect: () => void;
    joinVoice: (guildId: string, channelId: string) => void;
    leaveGuildVoice: (guildId: string) => void;
}

const getConnection = () => {
    return new HubConnectionBuilder()
        .withAutomaticReconnect()
        .withUrl(hostUrl()+"/api/v1/voice")
        .build();
}

const createVoiceStore = (): VoiceStoreState => {
    let state = $state<VoiceStoreState["state"]>("pending");
    let guilds = $state<DiscordGuildDto[]>([]);
    let voiceChannels = $state<DiscordChannelDto[]>([]);

    const connection = getConnection();
    
    connection.onclose(() => {
        state = "disconnected";
        guilds = [];
        voiceChannels = [];
    });
    connection.onreconnecting(() => state = "pending");
    connection.onreconnected(async () => {
        state = "connected";
        await connection.invoke("GetGuildData");
    });
    
    connection.on("Guilds", (newGuilds: DiscordGuildDto[]) => guilds = newGuilds);
    connection.on("ConnectedVoiceChannels", (newChannels: DiscordChannelDto[]) => voiceChannels = newChannels);

    return {
        get state() {
            return state;
        },
        get guilds() {
            return guilds;
        },
        get voiceChannels() {
            return voiceChannels;
        },
        async connect() {
            if (state !== "pending") {
                return;
            }

            try {
                await connection.start();
                await connection.invoke("GetGuildData");
                state = "connected";
            } catch (error) {
                state = "disconnected";
            }
        },
        async joinVoice(guildId: string, channelId: string) {
            await connection.invoke("ConnectToVoice", guildId, channelId);
        },
        async leaveGuildVoice(guildId: string) {
            await connection.invoke("LeaveGuildVoice", guildId);
        },
    };
}

const STORE_CTX = 'VOICE_CTX';

export const getVoiceStore = () => {
    return getContext<VoiceStoreState>(STORE_CTX);
}

export const initiateVoiceStore = () => {
    let store = getVoiceStore();

    if (!store) {
        store = createVoiceStore();
        setContext(STORE_CTX, store);
    }

    return store;
}