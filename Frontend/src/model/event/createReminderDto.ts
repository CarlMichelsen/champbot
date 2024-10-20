export type CreateReminderDto = {
    eventId: number;
    reminderNote?: string;
    minutesBeforeEvent: number;
}