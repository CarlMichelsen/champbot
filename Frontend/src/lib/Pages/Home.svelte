<script lang="ts">
    import { getUserStore } from "../../store/userStore.svelte";

    const userStore = getUserStore();
</script>

<p>Home</p>

<pre>{JSON.stringify(userStore.user, null, 2)}</pre>