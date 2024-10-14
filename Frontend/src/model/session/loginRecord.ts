export type LoginRecord = {
    loginId: string;
    refreshRecords: RefreshRecord[];
    createdUtc: Date;
    ip: string;
    userAgent: string;
    invalidatedUtc?: Date;
}

export type RefreshRecord = {
    refreshId: string;
    accessRecords: AccessRecord[];
    expiresUtc: Date;
    createdUtc: Date;
    ip: string;
    userAgent: string;
    invalidatedUtc?: Date;
}

export type AccessRecord = {
    accessId: string;
    expiresUtc: Date;
    createdUtc: Date;
    ip: string;
    userAgent: string;
}