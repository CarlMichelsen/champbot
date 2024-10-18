import type { CreateReminderDto } from "../../model/event/createReminderDto";
import type { EditReminderDto } from "../../model/event/editReminderDto";
import type { ReminderDto } from "../../model/event/reminderDto";
import { BaseClient } from "../baseClient";
import { hostUrl } from "../endpoints";

export class ReminderClient extends BaseClient
{
    public readonly reminderPath: string = "api/v1/Event/Reminder";
    
    protected host: string = hostUrl();

    public async createReminder(createReminder: CreateReminderDto) {
        return await this.request<ReminderDto>(
            "POST",
            this.reminderPath,
            createReminder);
    }

    public async editReminder(editReminder: EditReminderDto) {
        return await this.request<ReminderDto>(
            "PUT",
            this.reminderPath,
            editReminder);
    }

    public async removeReminder(eventId: number, reminderId: number) {
        return await this.request<void>(
            "DELETE",
            `${this.reminderPath}/${eventId}/${reminderId}`);
    }
}