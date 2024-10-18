export type CreateReminderDto = {
    eventId: string;
    reminderNote?: string;
    minutesBeforeEvent: number;
}