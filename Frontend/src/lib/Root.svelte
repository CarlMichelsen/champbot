<script lang="ts">
    import { link } from "svelte-routing";
    import { getUserStore } from "../store/userStore.svelte";
    import { UserClient } from "../util/clients/userClient";

    const userClient = new UserClient();
    const userStore = getUserStore();

    const logout = async () => {
        const logout = await userClient.logout();
        if (logout.ok) {
            userStore.logout();
        }
    }
</script>

<div class="container mx-auto grid grid-rows-[3rem_auto] gap-4 min-h-screen">
    <header class="grid grid-cols-[auto_6rem]">
        {#if userStore.state === "loggedIn"}
        <nav class="grid grid-cols-12">
            <a class="hover:underline pt-2.5" use:link href="/">Home</a>
            <a class="hover:underline pt-2.5" use:link href="account">Account</a>
        </nav>

        <div>
            <button
                onclick={() => logout()}
                class="block mx-auto mt-2.5 hover:underline">Logout</button>
        </div>
        {:else}
            <div></div>
            <div></div>
        {/if}
    </header>

    <main>
        <slot></slot>
    </main>
</div>