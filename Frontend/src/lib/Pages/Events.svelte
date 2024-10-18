<script lang="ts">
    import { getEventStore } from "../../store/eventStore.svelte";
    import { getUserStore } from "../../store/userStore.svelte";
    import { EventClient } from "../../util/clients/eventClient";
    import EventList from "../Components/EventList.svelte";

    const eventClient = new EventClient();
    const userStore = getUserStore();
    const eventStore = getEventStore();

    const attemptGetEvents = async () => {
        if (eventStore.state === "data") {
            return;
        }

        const res = await eventClient.getEvents();
        if (res.ok && !!res.value) {
            eventStore.setEvents(res.value);
        } else {
            eventStore.declareError();
        }
    }

    attemptGetEvents();
</script>

{#if userStore.state === "loggedIn"}
    {#if eventStore.state === "data"}
        <EventList />
    {:else if eventStore.state === "no-data"}
        <p>error</p>
    {:else}
        <p>loading...</p>
    {/if}
{:else}
    <p>Login to see reminders</p>
{/if}