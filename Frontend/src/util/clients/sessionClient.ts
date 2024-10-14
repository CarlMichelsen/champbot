import type { ServiceResponse } from "../../model/serviceResponse";
import type { LoginRecord } from "../../model/session/loginRecord";
import { BaseClient } from "../baseClient";
import { identityUrl } from "../endpoints";

export class SessionClient extends BaseClient
{
    public readonly sessionPath: string = "api/v1/session";

    protected host: string = identityUrl();

    public async getSessions() : Promise<ServiceResponse<LoginRecord[]>>
    {
        return await this.request<LoginRecord[]>("GET", this.sessionPath);
    }

    public async invalidateSessions(loginIds: string[]) : Promise<ServiceResponse<LoginRecord[]>>
    {
        const ids = loginIds.join(',');
        return await this.request<LoginRecord[]>("DELETE", `${this.sessionPath}/${ids}`);
    }
}