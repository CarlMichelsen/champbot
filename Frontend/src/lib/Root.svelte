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
    <header class="grid grid-cols-[auto_8rem]">
        {#if userStore.state === "loggedIn"}
        <nav class="pt-2.5 space-x-8">
            <a class="hover:underline" use:link href="/">Home</a>
            <a class="hover:underline" use:link href="/events">Events</a>
            <a class="hover:underline" use:link href="/discord">Discord</a>
            <a class="hover:underline" use:link href="/account">Account</a>
        </nav>

        <div class="grid grid-cols-[auto_3rem]">
            <div class="grid grid-rows-2">
                <p>{userStore.user?.username}</p>

                <button
                    onclick={() => logout()}
                    class="block mx-auto hover:underline text-xs px-2">Logout</button>
            </div>

            <img src={userStore.user?.avatarUrl} alt="profile">
        </div>
        {:else}
            <div></div>
            <div></div>
        {/if}
    </header>

    <main class="overflow-x-scroll hide-scrollbar">
        <slot></slot>
    </main>
</div>