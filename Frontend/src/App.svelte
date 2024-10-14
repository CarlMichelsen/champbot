<script lang="ts">
  import { Router, Route } from "svelte-routing";
  import Root from "./lib/Root.svelte";
    import type { AuthenticatedUser } from "./model/user/authenticatedUser";
    import { UserClient } from "./util/clients/userClient";

  type AppProps = {
    url: string;
  };

  type LoginState = {
    user?: AuthenticatedUser,
    state: "pending"|"loggedIn"|"loggedOut"
  };

  let loginState = $state<LoginState>({ state: "pending" });
  let { url }: AppProps = $props();

  const userClient = new UserClient();

  const attemptLogin = async () => {
    const userResponse = await userClient.getUser();
    if (userResponse.ok) {
      loginState = { state: "loggedIn", user: userResponse.value };
    } else {
      const refreshResponse = await userClient.refresh();
      if (refreshResponse.ok)
      {
        const reUserResponse = await userClient.getUser();
        if (reUserResponse.ok) {
          loginState = { state: "loggedIn", user: undefined };
        }
      }

      loginState = { state: "loggedOut", user: undefined };
    }
  }

  attemptLogin();
</script>

<Root>
  <Router {url}>
    <Route path="/">
      <p>home</p>
    </Route>

    <Route path="/idp">
      <p>idp</p>
    </Route>
  </Router>
</Root>