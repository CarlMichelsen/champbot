import type { ReminderDto } from "./reminderDto";

export type EventDto = {
    id: string;
    eventName: string;
    reminders: ReminderDto[]
    createdUtc: Date;
}