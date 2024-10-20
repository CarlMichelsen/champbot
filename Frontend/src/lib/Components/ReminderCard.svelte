<script lang="ts">
    import type { EventDto } from "../../model/event/eventDto";
    import type { ReminderDto } from "../../model/event/reminderDto";
    import { ReminderClient } from "../../util/clients/reminderClient";
    import { Format } from "../../util/format";
    import EditableTimeSpanField from "./EditableTimeSpanField.svelte";

    type ReminderCardProps = {
        event: EventDto;
        reminder: ReminderDto;
        deletedReminder: (reminder: ReminderDto) => void;
        editedReminder: (reminder: ReminderDto) => void;
    }

    let { event, reminder, deletedReminder, editedReminder }: ReminderCardProps = $props();
    const reminderClient = new ReminderClient();

    const subtractMinutes = (date: Date, minutes: number): Date => {
        const newDate = new Date(date);
        newDate.setMinutes(newDate.getMinutes() - minutes);
        return newDate;
    }

    const deleteReminder = async () => {
        const res = await reminderClient.removeReminder(reminder.eventId, reminder.id);
        if (res.ok) {
            deletedReminder(reminder);
        }
    }

    const editReminderMinutes = async (newMinutes: number) => {
        const res = await reminderClient.editReminder({
            eventId: reminder.eventId,
            reminderId: reminder.id,
            minutesBeforeEvent: newMinutes,
        });
        
        if (res.ok) {
            editedReminder({
                ...reminder,
                minutesBeforeEvent: newMinutes,
            });
        }
    }
    
    const eventTime = new Date(event.eventTimeUtc);
    const reminderTime = subtractMinutes(eventTime, reminder.minutesBeforeEvent);
    const reminderDateString = reminderTime.toLocaleDateString(undefined, Format.dateTimeOptions);
</script>

<div>
    <div class="grid grid-cols-[auto_1rem] gap-6">
        <time
            class="sr-only"
            datetime={reminderTime.toString()}>
            {reminderDateString}
        </time>
        
        
        <span title={reminderDateString}>
            <EditableTimeSpanField
                id={`reminder-timespan-${reminder.id}`}
                label="New reminder timespan"
                minutes={reminder.minutesBeforeEvent}
                edited={async (min) => {
                    await editReminderMinutes(min)
                }}/>
        </span>

        <button
            onclick={() => deleteReminder()}
            class="rounded-sm bg-red-500 hover:bg-red-800 hover:underline text-xs aspect-square w-full">
            X
        </button>
    </div>
    

    {#if !!reminder.reminderNote}
        <p class="text-xs">{reminder.reminderNote}</p>
    {/if}
</div>