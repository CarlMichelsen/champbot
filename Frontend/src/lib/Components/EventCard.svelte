<script lang="ts">
    import type { EditEventDto } from "../../model/event/editEventDto";
    import type { EventDto } from "../../model/event/eventDto";
    import type { ReminderDto } from "../../model/event/reminderDto";
    import { EventClient } from "../../util/clients/eventClient";
    import { ReminderClient } from "../../util/clients/reminderClient";
    import EditableDateTimeField from "./EditableDateTimeField.svelte";
    import EditableTextField from "./EditableTextField.svelte";
    import EditableTimeSpanField from "./EditableTimeSpanField.svelte";
    import ReminderCard from "./ReminderCard.svelte";

    const client = new EventClient();

    type EventCardProps = {
        event: EventDto;
        deletedEvent: (event: EventDto) => void;
        editedEvent: (event: EventDto) => void;

        deletedReminder: (reminder: ReminderDto) => void;
        editedReminder: (reminder: ReminderDto) => void;
        addedReminder: (reminder: ReminderDto) => void;
    }

    let {
        event,
        deletedEvent,
        editedEvent,
        deletedReminder,
        editedReminder,
        addedReminder
    }: EventCardProps = $props();

    let addingReminder = $state<boolean>(false);
    let newReminderMinutesBefore = $state<number>(2);

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

    const editDate = async (newDate: Date) => {
        const payload: EditEventDto = {
            eventId: event.id,
            eventTimeUtc: newDate,
        };

        const res = await client.editEvent(payload);
        if (res.ok && !!res.value) {
            editedEvent(res.value);
        }
    }

    const editReminder = async (newReminder: ReminderDto) => {
        const reminderClient = new ReminderClient();
        const res = await reminderClient.editReminder({
            eventId: newReminder.eventId,
            reminderId: newReminder.id,
            reminderNote: newReminder.reminderNote,
            minutesBeforeEvent: newReminder.minutesBeforeEvent,
        });
        
        if (res.ok && !!res.value) {
            editedReminder(res.value);
        }
    }
</script>

<div class="inline-block p-1 rounded-sm bg-neutral-700">
    <div class="grid grid-cols-[auto_1.5rem] gap-8">
        <EditableTextField
            id={`event-${event.id}-title`}
            label="Event Title"
            additionalClass="text-lg"
            text={event.eventName}
            edited={text => editTitle(text)}/>

        <button
            onclick={() => deleteEvent()}
            class="rounded-sm bg-red-500 hover:bg-red-800 hover:underline text-xs aspect-square w-full">
            X
        </button>
    </div>

    <EditableDateTimeField
        id={`event-${event.id}-time`}
        label="Event Time"
        date={new Date(event.eventTimeUtc)}
        edited={editDate} />

    <br>

    <button
        onclick={() => addingReminder = !addingReminder}
        class="disabled:hidden hover:font-bold rounded-sm w-6">
        🔔
    </button>

    {#if event.reminders.length > 0 || addingReminder}
        <hr class="my-2">

        <ol class="list-disc ml-6 space-y-2">
            {#if addingReminder}
                <li class="bg-neutral-900 rounded-md p-1">
                    <EditableTimeSpanField
                        id="new-reminder"
                        label="Add new reminder"
                        minutes={newReminderMinutesBefore}
                        edited={(e) => newReminderMinutesBefore = e} />
                    
                    <br>

                    <button
                        onclick={async () => {
                            const client = new ReminderClient();
                            const res = await client.createReminder({
                                eventId: event.id,
                                minutesBeforeEvent: newReminderMinutesBefore,
                            });

                            if (res.ok && res.value) {
                                addedReminder(res.value);
                                addingReminder = false;
                            }
                        }}
                        class="hover:underline">
                        Add
                    </button>
                </li>
            {/if}

            {#each event.reminders as reminder}
                <li id={`reminder-id-${reminder.id}`}>
                    <ReminderCard
                        event={event}
                        reminder={reminder}
                        deletedReminder={deletedReminder}
                        editedReminder={editReminder} />
                </li>
            {/each}
        </ol>
    {/if}
</div>
