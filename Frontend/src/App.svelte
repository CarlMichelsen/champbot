<script lang="ts">
  import { Router, Route } from "svelte-routing";
  import Root from "./lib/Root.svelte";
  import { getUserStore, initiateUserStore } from "./store/userStore.svelte";
  import Home from "./lib/Pages/Home.svelte";
  import Account from "./lib/Pages/Account.svelte";
  import LoggedOut from "./lib/Pages/LoggedOut.svelte";
  import Events from "./lib/Pages/Events.svelte";
  import { UserAccessor } from "./util/userAccessor";
  import { initiateEventStore } from "./store/eventStore.svelte";
  import DiscordMessage from "./lib/Pages/DiscordMessage.svelte";
  import DiscordVoice from "./lib/Pages/DiscordVoice.svelte";
  import Discord from "./lib/Pages/Discord.svelte";
    import { initiateVoiceStore } from "./store/voiceStore.svelte";

  type AppProps = {
    url: string;
  };

  let { url }: AppProps = $props();

  initiateUserStore();
  initiateEventStore();
  initiateVoiceStore();

  const userStore = getUserStore();

  const attemptLogin = async () => {
    const userResponse = await UserAccessor.getUser();
    if (userResponse.ok) {
      userStore.login(userResponse.value!);
    } else {
      const refreshResponse = await UserAccessor.refresh();
      if (refreshResponse.ok) {
        const secondUserResponse = await UserAccessor.getUser();
        if (secondUserResponse.ok) {
          userStore.login(secondUserResponse.value!);
          return;
        }
      }

      userStore.logout();
    }
  }

  attemptLogin();
</script>


<Root>
  {#if userStore.state === "loggedIn"}
    <Router {url}>
      <Route path="/">
        <Home />
      </Route>

      <Route path="/events">
        <Events />
      </Route>

      <Route path="/discord">
        <Discord />
      </Route>
      
      <Route path="/discord/message">
        <DiscordMessage />
      </Route>

      <Route path="/discord/voice">
        <DiscordVoice />
      </Route>
      

      <Route path="/account">
        <Account />
      </Route>
    </Router>
  {:else if userStore.state === "loggedOut"}
    <LoggedOut />
  {:else if userStore.state === "pending"}
    <p>pending...</p>
  {:else}
    <p>unknown state</p>
  {/if}
</Root>