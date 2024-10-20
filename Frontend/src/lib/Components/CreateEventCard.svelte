<script lang="ts">
    import { getEventStore } from "../../store/eventStore.svelte";
    import { EventClient } from "../../util/clients/eventClient";
    import { isValidDate } from "../../util/dateUtil";
    import EditableDateTimeField from "./EditableDateTimeField.svelte";
    import EditableTextField from "./EditableTextField.svelte";

    let touched = $state<boolean>(false);
    let title = $state<string>("Event Title");
    let date = $state<Date>(new Date());
    
    const eventStore = getEventStore();
    const client = new EventClient();

    const editDate = (newDate: Date) => {
        if (isValidDate(newDate)) {
            touched = true;
            date = newDate;
        }
    }

    const editTitle = (newTitle: string) => {
        if (newTitle.length > 2) {
            touched = true;
            title = newTitle;
        }
    }

    const createEvent = async () => {
        const res = await client.createEvent({
            eventName: title,
            eventTimeUtc: date,
        });

        if (res.ok && res.value) {
            eventStore.addEvent(res.value);
            touched = false;
        }
    }
</script>

<div class="inline-block p-1 rounded-sm bg-neutral-700">
    <h3 class="text-xl underline">Create new event</h3>

    <br>

    <EditableTextField
        id="create-event-title"
        label="Event Title"
        additionalClass="text-lg"
        text={title}
        edited={editTitle}/>

    <br>

    <EditableDateTimeField
        id="create-event-time"
        label="Event Time"
        date={new Date(date)}
        edited={editDate} />
    
    <br>
    <br>

    <button
        disabled={!touched}
        onclick={() => createEvent()}
        class="disabled:hidden bg-green-900 hover:bg-green-700 hover:font-bold rounded-sm w-full h-12">
        <p class="text-3xl -mt-1 ">Create</p>
    </button>
</div>