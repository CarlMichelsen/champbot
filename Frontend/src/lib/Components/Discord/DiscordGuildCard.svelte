<script lang="ts">
    import type { DiscordGuildDto } from "../../../model/discord/discordGuildDto";

    type DiscordGuildCardProps = {
        guild: DiscordGuildDto;
    }

    let { guild }: DiscordGuildCardProps = $props();
</script>

<div>
    <p>Guildname: {guild.guildName}</p>
</div>