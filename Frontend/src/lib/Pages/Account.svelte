<script lang="ts">
    import type { LoginRecord } from "../../model/session/loginRecord";
    import { SessionClient } from "../../util/clients/sessionClient";

    type AccountPageState = {
        loginRecords: LoginRecord[]
    }

    let accountPageState = $state<AccountPageState|null>(null);
    let selectedLogins = $state<string[]>([]);

    const dateTimeOptions: Intl.DateTimeFormatOptions = {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    };
    const client = new SessionClient();
    const loginSelected = (loginRecord: LoginRecord) => {
        if (!!selectedLogins.find(sl => sl === loginRecord.loginId)) {
            selectedLogins = selectedLogins.filter(sl => sl !== loginRecord.loginId);
        } else {
            selectedLogins = [ ...selectedLogins, loginRecord.loginId ];
        }
    }

    const getAccountPageDate = async () => {
        const sessionsResponse = await client.getSessions();
        if (sessionsResponse.ok) {
            accountPageState = { loginRecords: sessionsResponse.value! };
        }
    }

    const invalidateSelectedSessions = async () => {
        const invalidateSessionsResult = await client.invalidateSessions(selectedLogins);
        if (invalidateSessionsResult.ok) {
            accountPageState = {
                loginRecords: accountPageState?.loginRecords.filter(lr =>
                    !invalidateSessionsResult.value!.find(srv => srv.loginId === lr.loginId))!,
            };
            selectedLogins = [];
        }
    }

    getAccountPageDate();
</script>

{#if accountPageState !== null}
    <h2 class="text-lg">Active login records</h2>

    <ol class="space-y-6">
        {#each accountPageState.loginRecords as loginRecord}
            <li class="outline outline-1 outline-black rounded-sm p-1 grid grid-cols-[5rem_auto_18rem_10rem]">
                <div class="grid grid-row-2">
                    <div class="grid grid-cols-[1rem_auto] ml-1">
                        <label for={"login-checkbox-"+loginRecord.loginId} class="sr-only">Select login</label>
                        <input
                            onclick={() => loginSelected(loginRecord)}
                            checked={!!selectedLogins.find(sl => sl == loginRecord.loginId)}
                            class="block text-left"
                            type="checkbox"
                            name={"login-checkbox-name-"+loginRecord.loginId}
                            id={"login-checkbox-"+loginRecord.loginId}>
                        <div></div>
                    </div>
                    <p class="mb-1">|{loginRecord.loginId}|</p>
                </div>
                
                <p>{loginRecord.userAgent}</p>
                <p>Created {(new Date(loginRecord.createdUtc)).toLocaleDateString(undefined, dateTimeOptions)}</p>
                <p>{loginRecord.ip}</p>

                <h2 class="ml-8 col-span-4 underline">Refresh records</h2>

                <ol class="ml-8 col-span-4 space-y-3">
                    {#each loginRecord.refreshRecords as refreshRecord}
                        <li class="grid grid-cols-[5rem_auto_18rem_10rem]">
                            <p>|{refreshRecord.refreshId}|</p>
                            <p>{refreshRecord.userAgent}</p>
                            <p>Expires {(new Date(refreshRecord.expiresUtc)).toLocaleDateString(undefined, dateTimeOptions)}</p>
                            <p>{refreshRecord.ip}</p>

                            <h2 class="ml-8 col-span-4 underline">Access records</h2>

                            <ol class="ml-8 col-span-4">
                                {#each refreshRecord.accessRecords as accessRecord}
                                    <li class="grid grid-cols-[5rem_auto_18rem_10rem]">
                                        <p>|{accessRecord.accessId}|</p>
                                        <p>{accessRecord.userAgent}</p>
                                        <p>Expires {(new Date(accessRecord.expiresUtc)).toLocaleDateString(undefined, dateTimeOptions)}</p>
                                        <p>{accessRecord.ip}</p>
                                    </li>
                                {/each}
                            </ol>
                        </li>
                    {/each}
                </ol>
            </li>
        {/each}
    </ol>

    {#if selectedLogins.length > 0}
        <br>
        <button
            onclick={() => invalidateSelectedSessions()}
            class="p-2 bg-red-400 hover:bg-red-600 hover:underline rounded-sm">
            Invalidate login{selectedLogins.length > 1 ? "s" : ""}
        </button>
    {/if}
{:else}
    <p>pending...</p>
{/if}