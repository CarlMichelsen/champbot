<script lang="ts">
    import type { DiscordGuildDto } from "../../../model/discord/discordGuildDto";
    import type { DiscordChannelDto } from "../../../model/discord/discordChannelDto";
    import { DiscordClient } from "../../../util/clients/discordClient";

    let sending = $state<boolean>(false);
    let area = $state<HTMLTextAreaElement>();

    let selectedGuild = $state<DiscordGuildDto|null>(null);
    let selectedChannel = $state<DiscordChannelDto|null>(null);
    let discordMessage = $state<string>("");

    let guilds = $state<DiscordGuildDto[]|null>(null);

    const client = new DiscordClient();

    const attemptGetGuilds = async () => {
        const gs = await client.getGuilds();
        if (gs.ok && gs.value) {
            selectedGuild = gs.value[0] ?? null;
            selectedChannel = selectedGuild.textChannels[0] ?? null;
            guilds = gs.value;
        }
    }

    const sendMessage = async () => {
        sending = true;
        var res = await client.sendMessage({
            guildId: selectedGuild!.guildId,
            textChannelId: selectedChannel!.channelId,
            message: discordMessage,
        });

        if (res.ok) {
            discordMessage = "";
        }

        sending = false;
        setTimeout(() => area?.focus(), 0);
    }

    attemptGetGuilds();
</script>

{#if guilds !== null}
    <h3 class="text-xl mb-8">Send discord message</h3>

    <label for="guild-selector">Guild</label>
    <select
        name="guild-selector"
        id="guild-selector"
        class="dark:bg-neutral-700 p-1 rounded-sm"
        value={selectedGuild!.guildId}
        oninput={(e) => {
            selectedGuild = guilds?.find(g => g.guildId == e.currentTarget.value) ?? null
            selectedChannel = selectedGuild?.textChannels[0] ?? null;
        }}>
        {#each guilds as guild}
            <option value={guild.guildId}>{guild.guildName}</option>
        {/each}
    </select>

    {#if selectedGuild}
        <p class="text-xs italic ml-12">{selectedGuild.guildId}</p>
    {/if}

    {#if !!selectedGuild}
        <br>
        <br>

        <label for="channel-selector">Channel</label>
        <select
            name="channel-selector"
            id="channel-selector"
            class="dark:bg-neutral-700 p-1 rounded-sm"
            value={selectedChannel!.channelId}
            oninput={(e) => {
                selectedChannel = selectedGuild?.textChannels.find(c => c.channelId == e.currentTarget.value) ?? null
            }}>
            {#each selectedGuild.textChannels as channel}
                <option value={channel.channelId}>
                    {#if !!channel.category}
                        {channel.category}:
                    {/if}
                    {channel.channelName}
                </option>
            {/each}
        </select>

        {#if selectedChannel}
        <p class="text-xs italic ml-16">{selectedChannel.channelId}</p>
        {/if}
    {/if}

    {#if !!selectedChannel}
        <br>
        <br>

        <textarea
            bind:this={area}
            bind:value={discordMessage}
            disabled={sending}
            class="dark:bg-neutral-700 resize-none focus:outline-none p-1 w-96 min-h-32 disabled:bg-yellow-900"
            name="message-area"
            id="message-area"
            onkeydown={async (e) => {
                if (e.key === 'Enter' && !e.shiftKey) {
                    e.preventDefault();
                    await sendMessage();
                }
            }}></textarea>

        <br>
        
        <button
            class="hover:underline"
            onclick={async () => await sendMessage()}>
            Send message!
        </button>

    {/if}
{:else}
    <p>No guilds!</p>
{/if}