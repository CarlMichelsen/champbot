<script lang="ts">
    import { getVoiceStore } from "../../store/voiceStore.svelte";

    const voiceStore = getVoiceStore();

    voiceStore.connect();

    const toggleConnect = async (guildId: string, channelId: string) => {
        const exsisting = voiceStore.voiceChannels.find(c => c.channelId);
        if (!!exsisting) {
            await voiceStore.leaveGuildVoice(guildId);
        } else {
            await voiceStore.joinVoice(guildId, channelId);
        }
    }

    const connectedToChannel = (channelId: string): boolean => {
        return !!voiceStore.voiceChannels.find(c => c.channelId);
    }
</script>

{#if voiceStore.state === "connected"}
<ol>
    {#each voiceStore.guilds as g}
        <li>
            <p>{g.guildName}</p>
        </li>

        {#each g.voiceChannels as channel}
        <li class="ml-12">
            <button
                class={`${connectedToChannel(channel.channelId) ? "bg-green-800" : "bg-none"} p-2 rounded-sm`}
                onclick={() => toggleConnect(g.guildId, channel.channelId)}>{channel.channelName}</button>
        </li>
        {/each}
    {/each}
</ol>
{:else}
    <p>{voiceStore.state}</p>
{/if}
<!--<pre>{JSON.stringify(voiceStore.guilds, null, 2)}</pre>-->