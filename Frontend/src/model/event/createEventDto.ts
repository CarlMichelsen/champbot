export type CreateEventDto = {
    eventName: string;
    reminderNote?: string;
    EventTimeUtc: Date;
}