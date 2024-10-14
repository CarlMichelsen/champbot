<script lang="ts">
    import { UserClient } from "../../util/clients/userClient";

    const client = new UserClient();

    const login = () => {
        location.replace(client.getLoginUrl());
    }
</script>

<button class="hover:underline" onclick={() => login()}>Login</button>

