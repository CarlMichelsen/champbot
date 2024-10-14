export const hostUrl = (): string => import.meta.env.VITE_APP_ENV === 'development'
    ? "http://localhost:5667"
    : "";

export const identityUrl = (): string => import.meta.env.VITE_APP_ENV === 'development'
    ? "http://localhost:5791"
    : "identity.survivethething.com";