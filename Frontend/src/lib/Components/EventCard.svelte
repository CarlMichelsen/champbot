<script lang="ts">
    import type { EditEventDto } from "../../model/event/editEventDto";
    import type { EventDto } from "../../model/event/eventDto";
    import { EventClient } from "../../util/clients/eventClient";
    import { Format } from "../../util/format";
    import EditableTextField from "./EditableTextField.svelte";
    import ReminderCard from "./ReminderCard.svelte";

    const client = new EventClient();

    type EventCardProps = {
        event: EventDto;
        deletedEvent: (event: EventDto) => void;
        editedEvent: (event: EventDto) => void;
    }

    let { event, deletedEvent, editedEvent }: EventCardProps = $props();

    const deleteEvent = async () => {
        const res = await client.deleteEvent(event.id);
        if (res.ok) {
            deletedEvent(event);
        } else {
            console.error(res.errors);
        }
    }

    const editTitle = async (newTitle: string) => {
        const payload: EditEventDto = {
            eventId: event.id,
            name: newTitle,
        };

        const res = await client.editEvent(payload);
        if (res.ok && !!res.value) {
            editedEvent(res.value);
        }
    }
</script>

<div class="inline-block p-1">
    <div class="grid grid-cols-[auto_1.5rem] gap-8">
        <EditableTextField
            additionalClass="text-lg"
            text={event.eventName}
            edited={text => editTitle(text)}/>

        <button
            onclick={() => deleteEvent()}
            class="rounded-sm bg-red-500 hover:bg-red-800 hover:underline text-xs aspect-square w-full">
            X
        </button>
    </div>

    <p>
        <time
            class="rounded-sm py-1"
            datetime={event.eventTimeUtc.toString()}>
            {new Date(event.eventTimeUtc).toLocaleDateString(undefined, Format.dateTimeOptions)}
        </time>
    </p>

    

    {#if event.reminders.length > 0}
        <hr class="my-2">

        <h3 class="text-lg">Reminders</h3>

        <ol class="list-disc ml-6 space-y-2">
            {#each event.reminders as reminder}
                <li id={`reminder-id-${reminder.id}`}>
                    <ReminderCard
                        event={event}
                        reminder={reminder}
                        deletedReminder={(deleted) => event.reminders = event.reminders.filter(e => e.id !== deleted.id)} />
                </li>
            {/each}
        </ol>
    {/if}
</div>
