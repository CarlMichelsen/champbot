import { hostUrl } from "../endpoints";
import { BaseClient } from "../baseClient";
import type { EventDto } from "../../model/event/eventDto";
import type { CreateEventDto } from "../../model/event/createEventDto";
import type { EditEventDto } from "../../model/event/editEventDto";

export class EventClient extends BaseClient
{
    public readonly eventPath: string = "api/v1/Event";
    
    protected host: string = hostUrl();
    
    public async getEvents() {
        return await this.request<EventDto[]>("GET", this.eventPath);
    }

    public async createEvent(createEvent: CreateEventDto) {
        return await this.request<EventDto>("POST", this.eventPath, createEvent);
    }

    public async editEvent(editEvent: EditEventDto) {
        return await this.request<EventDto>("PUT", this.eventPath, editEvent);
    }

    public async deleteEvent(eventId: number) {
        return await this.request<void>("DELETE", `${this.eventPath}/${eventId}`);
    }
}