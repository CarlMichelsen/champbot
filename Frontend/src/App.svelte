<script lang="ts">
  import { Router, Route } from "svelte-routing";
  import Root from "./lib/Root.svelte";

  type AppProps = {
    url: string;
  };

  let { url }: AppProps = $props();
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