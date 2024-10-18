export type ReminderDto = {
    id: number;
    eventId: number;
    reminderNote: string;
    minutesBeforeEvent: number;
    reminded: boolean;
    createdUtc: Date;
}