export type EditReminderDto = {
    eventId: string;
    reminderId: string;
    reminderNote?: string;
    timeBeforeEvent?: string; // TODO: find a nice way to convert to and from TimeSpan
}