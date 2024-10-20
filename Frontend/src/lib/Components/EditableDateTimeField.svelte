<script lang="ts">
    import { isValidDate } from "../../util/dateUtil";
    import { Format } from "../../util/format";

    type EditableDateTimeFieldProps = {
        id: string;
        label: string;
        date: Date;
        additionalClass?: string;
        edited: (date: Date) => void;
    };

    let { id, label, edited, additionalClass, date }: EditableDateTimeFieldProps = $props();

    let inputField = $state<HTMLInputElement|null>(null);
    let editing = $state<boolean>(false);

    const startEditing = () => {
        editing = true;
        setTimeout(() => inputField?.focus(), 0);
    }

    const stopEditing = async (e: { currentTarget: EventTarget & HTMLInputElement; }) => {
        const elem = e.currentTarget as HTMLInputElement;
        const newDate = new Date(elem.value);
        if (isValidDate(newDate) && date.toString() !== newDate.toString()) {
            edited(newDate);
        }
        editing = false;
    }
</script>

{#if editing}
    <label class="sr-only" for={id}>{label}</label>
    <input
        bind:this={inputField}
        id={id}
        class={`${additionalClass ?? ""} dark:bg-neutral-700 ml-4 p-1 rounded-sm`}
        type="datetime-local"
        name="datetime-picker"
        value={Format.formatDateToDateTimeLocal(date)}
        onblur={(e) => stopEditing(e)}
        onkeydown={(e) => {
            if (e.key === 'Enter') {
                e.preventDefault();
                stopEditing(e);
            }
        }}/>
{:else}
    <button
        id={id}
        class="p-1"
        onclick={() => startEditing()}>
        <span class="inline-block text-xs w-4">🖊</span>
        <time
            class={`${additionalClass ?? ""}`}
            datetime={date.toString()}>
            {new Date(date).toLocaleDateString(undefined, Format.dateTimeOptions)}
        </time>
    </button>
{/if}