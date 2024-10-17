export type ReminderDto = {
    id: string;
    eventId: string;
    reminderNote: string;
    timeBeforeEvent: string; // TODO: find a nice way to convert to and from TimeSpan
    reminded: boolean;
    createdUtc: Date;
}