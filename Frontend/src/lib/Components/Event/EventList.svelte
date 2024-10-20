<script lang="ts">
    import { getEventStore } from "../../../store/eventStore.svelte";
    import CreateEventCard from "./CreateEventCard.svelte";
    import EventCard from "./EventCard.svelte";

    const eventStore = getEventStore();
</script>

<div class="grid lg:grid-cols-2 xl:grid-cols-3 gap-x-10 gap-y-20">
    <CreateEventCard />
    {#each eventStore.events as e}
        <EventCard
            event={e}
            deletedEvent={e => eventStore.deleteEvent(e)}
            editedEvent={e => eventStore.editEvent(e)}
            deletedReminder={r => eventStore.deleteReminder(r)}
            editedReminder={r => eventStore.editReminder(r)}
            addedReminder={r => eventStore.addReminder(r)} />
    {/each}
</div>