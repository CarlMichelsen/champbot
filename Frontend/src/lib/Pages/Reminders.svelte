<script lang="ts">
    import { getUserStore } from "../../store/userStore.svelte";
    import ReminderList from "../Components/ReminderList.svelte";

    const userStore = getUserStore();
</script>

{#if userStore.state === "loggedIn"}
    <ReminderList />
{:else}
    <p>Login to see reminders</p>
{/if}