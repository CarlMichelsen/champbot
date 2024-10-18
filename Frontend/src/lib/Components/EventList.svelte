<script lang="ts">
    import type { EventDto } from "../../model/event/eventDto";
    import { getEventStore } from "../../store/eventStore.svelte";
    import EventCard from "./EventCard.svelte";
    const eventStore = getEventStore();

    const deletedEvent = (event: EventDto) => {
        eventStore.deleteEvent(event);
    }

    const editedEvent = (event: EventDto) => {
        eventStore.editEvent(event);
    }
</script>

<h1 class="text-lg">Events</h1>

<br>

<div class="grid sm:grid-cols-2 xl:grid-cols-4 gap-x-10 gap-y-20">
    {#each eventStore.events as e}
        <EventCard
            event={e}
            deletedEvent={deletedEvent}
            editedEvent={editedEvent}/>
    {/each}
</div>