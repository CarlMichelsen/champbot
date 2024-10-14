const isDevelopment = () => import.meta.env.VITE_APP_ENV === 'development';

export const hostUrl = (): string => isDevelopment()
    ? "http://localhost:5667"
    : "";

export const identityUrl = (): string => isDevelopment()
    ? "http://localhost:5791"
    : "http://identity.survivethething.com";