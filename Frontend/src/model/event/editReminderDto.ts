export type EditReminderDto = {
    eventId: string;
    reminderId: string;
    reminderNote?: string;
    minutesBeforeEvent?: number;
}