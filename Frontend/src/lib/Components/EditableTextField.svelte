<script lang="ts">
    type EditableTextFieldProps = {
        text: string;
        additionalClass?: string;
        edited: (text: string) => void;
    }

    let inputField = $state<HTMLInputElement|null>(null);
    let editing = $state<boolean>(false);
    let { edited, additionalClass, text }: EditableTextFieldProps = $props();

    const fieldEdited = (e: Event & { currentTarget: EventTarget & HTMLInputElement; }) => {
        const elem = e.target as HTMLInputElement;
        if (text !== elem.value) {
            edited(elem.value);
        }
    }

    const stopEditing = (e: { currentTarget: EventTarget & HTMLInputElement; }) => {
        const elem = e.currentTarget as HTMLInputElement;
        if (text !== elem.value) {
            edited(elem.value);
        }
        editing = false;
    }

    const startEditing = () => {
        editing = true;
        setTimeout(() => inputField?.focus(), 0);
    }
</script>

{#if editing}
    <input
        bind:this={inputField}
        class={`${additionalClass} dark:bg-neutral-500 p-1 rounded-sm`}
        type="text"
        name="text-edit"
        oninput={fieldEdited}
        onkeydown={(e) => {
            if (e.key === 'Enter') {
                e.preventDefault();
                stopEditing(e);
            }
        }}
        onblur={stopEditing}
        value={text}>
{:else}
    <button
        class="text-left p-1"
        onclick={() => startEditing()}>

        <p class={`${additionalClass}`}>
            {text}
        </p>
    </button>
{/if}