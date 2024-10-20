export type EditReminderDto = {
    eventId: number;
    reminderId: number;
    reminderNote?: string;
    minutesBeforeEvent?: number;
}