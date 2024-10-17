<script lang="ts">
  import { Router, Route } from "svelte-routing";
  import Root from "./lib/Root.svelte";
  import { UserClient } from "./util/clients/userClient";
  import { getUserStore, initiateUserStore } from "./store/userStore.svelte";
  import Home from "./lib/Pages/Home.svelte";
  import Account from "./lib/Pages/Account.svelte";
  import LoggedOut from "./lib/Pages/LoggedOut.svelte";
    import Reminders from "./lib/Pages/Reminders.svelte";

  type AppProps = {
    url: string;
  };

  let { url }: AppProps = $props();

  initiateUserStore();

  const userStore = getUserStore();
  const userClient = new UserClient();

  const attemptLogin = async () => {
    const userResponse = await userClient.getUser();
    if (userResponse.ok) {
      userStore.login(userResponse.value!);
    } else {
      const refreshResponse = await userClient.refresh();
      if (refreshResponse.ok) {
        const reUserResponse = await userClient.getUser();
        if (reUserResponse.ok) {
          userStore.login(reUserResponse.value!);
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

      <Route path="/reminders">
        <Reminders />
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