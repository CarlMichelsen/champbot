import type { ReminderDto } from "./reminderDto";

export type EventDto = {
    id: number;
    creatorId: number;
    eventName: string;
    reminders: ReminderDto[]
    eventTimeUtc: Date;
    createdUtc: Date;
}