<script lang="ts">
    import { Format } from "../../util/format";

    type EditableTimeSpanFieldProps = {
        id: string;
        label: string;
        minutes: number;
        additionalClass?: string;
        edited: (minute: number) => void;
    };

    let { id, label, edited, additionalClass, minutes }: EditableTimeSpanFieldProps = $props();
    let inputField = $state<HTMLInputElement|null>(null);
    let editing = $state<boolean>(false);

    const fieldEdited = (e: Event & { currentTarget: EventTarget & HTMLInputElement; }) => {
        const elem = e.target as HTMLInputElement;
        if (minutes !== Number(elem.value)) {
            edited(Number(elem.value));
        }
    }

    const stopEditing = (e: { currentTarget: EventTarget & HTMLInputElement; }) => {
        const elem = e.currentTarget as HTMLInputElement;
        if (minutes !== Number(elem.value)) {
            edited(Number(elem.value));
        }
        editing = false;
    }

    const startEditing = () => {
        editing = true;
        setTimeout(() => inputField?.focus(), 0);
    }
</script>

{#if editing}
    <label class="sr-only" for={id}>{label}</label>
    <input
        bind:this={inputField}
        id={id}
        class={`${additionalClass ?? ""} dark:bg-neutral-700 ml-5 p-1 rounded-sm`}
        type="text"
        name="text-edit"
        oninput={fieldEdited}
        onkeydown={e => {
            if (e.key === 'Enter') {
                e.preventDefault();
                stopEditing(e);
            }
        }}
        onblur={stopEditing}
        value={minutes}>
{:else}
    <button
        class="text-left p-1"
        onclick={() => startEditing()}>
        <span class="inline-block text-xs w-4">🖊</span>
        <p class={`${additionalClass ?? ""} inline-block`}>
            {Format.minutesToTimeString(minutes)} before event
        </p>
    </button>
{/if}