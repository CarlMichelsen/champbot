import type { PublicUser } from "./publicUser";

export type AuthenticatedUser = {
    authenticationId: string;
    email: string;
} & PublicUser