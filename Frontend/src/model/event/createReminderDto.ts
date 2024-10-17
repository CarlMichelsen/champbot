export type CreateReminderDto = {
    eventId: string;
    reminderNote?: string;
    timeBeforeEvent: string; // TODO: find a nice way to send TimeSpan to server
}