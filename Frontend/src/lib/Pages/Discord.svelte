<script lang="ts">
    import { link } from "svelte-routing";
</script>

<ol class="space-y-6">
    <li>
        <a class="hover:underline" use:link href="discord/message"
            >Discord Message</a
        >
    </li>

    <li>
        <a class="hover:underline" use:link href="discord/voice"
            >Discord Voice</a
        >
    </li>
</ol>
