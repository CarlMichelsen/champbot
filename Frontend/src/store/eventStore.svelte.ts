import { getContext, setContext } from "svelte";
import type { EventDto } from "../model/event/eventDto";

export type EventDataState = "pending"|"no-data"|"data";

export type EventStoreState = {
    readonly state: EventDataState,
    readonly events: EventDto[],
    declareError: () => void;
    setEvents: (events: EventDto[]) => void;
    deleteEvent: (event: EventDto) => void;
    editEvent: (event: EventDto) => void;
}

const createEventStore = (): EventStoreState => {
    let state = $state<EventStoreState["state"]>("pending");
    let events = $state<EventDto[]>([]);

    return {
        get state() {
            return state;
        },
        get events() {
            return events;
        },
        declareError() {
            state = "no-data";
        },
        setEvents(newEvents: EventDto[]) {
            state = "data";
            events = newEvents;
        },
        deleteEvent(event: EventDto) {
            events = events.filter(e => e.id !== event.id);
        },
        editEvent(event: EventDto) {
            events = events.map(e => e.id === event.id ? event : e);
        }
    };
}

const STORE_CTX = 'EVENT_CTX';

export const getEventStore = () => {
    return getContext<EventStoreState>(STORE_CTX);
}

export const initiateEventStore = () => {
    let store = getEventStore();

    if (!store) {
        store = createEventStore();
        setContext(STORE_CTX, store);
    }

    return store;
}