<script lang="ts">
    import { getUserStore } from "../../store/userStore.svelte";

    const userStore = getUserStore();
</script>

<p>User data</p>
<div class="dark:bg-black bg-white">
    <pre>{JSON.stringify(userStore.user, null, 2)}</pre>
</div>