import type { LoginRecord } from "../../model/session/loginRecord";
import { BaseClient } from "../baseClient";
import { identityUrl } from "../endpoints";

export class SessionClient extends BaseClient
{
    public readonly sessionPath: string = "api/v1/Session";

    protected host: string = identityUrl();

    public async getSessions() {
        return await this.request<LoginRecord[]>("GET", this.sessionPath);
    }

    public async invalidateSessions(loginIds: string[]) {
        const ids = loginIds.join(',');
        return await this.request<LoginRecord[]>("DELETE", `${this.sessionPath}/${ids}`);
    }
}