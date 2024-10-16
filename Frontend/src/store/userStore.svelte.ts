import { getContext, setContext } from "svelte";
import type { AuthenticatedUser } from "../model/user/authenticatedUser"

export type LoginState = "pending"|"loggedIn"|"loggedOut";

export type UserStoreState = {
    readonly user?: AuthenticatedUser,
    readonly state: LoginState,
    login: (newUser: AuthenticatedUser) => void;
    logout: () => void;
}

const createUserStore = (): UserStoreState => {
    let user = $state<AuthenticatedUser|undefined>(undefined);
    let state = $state<UserStoreState["state"]>("pending");

    return {
        login(newUser: AuthenticatedUser) {
            user = newUser;
            state = "loggedIn";
        },
        logout() {
            user = undefined;
            state = "loggedOut";
        },
        get user() {
            return user;
        },
        get state() {
            return state;
        }
    };
}

const STORE_CTX = 'USER_CTX';

export const getUserStore = () => {
    return getContext<UserStoreState>(STORE_CTX);
}

export const initiateUserStore = () => {
    let store = getUserStore();

    if (!store) {
        store = createUserStore();
        setContext(STORE_CTX, store);
    }

    return store;
}