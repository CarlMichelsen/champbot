export class Format {
    public static readonly dateTimeOptions: Intl.DateTimeFormatOptions = {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    };

    public static minutesToTimeString(totalMinutes: number): string {
        const days = Math.floor(totalMinutes / (24 * 60));
        const hours = Math.floor((totalMinutes % (24 * 60)) / 60);
        const minutes = totalMinutes % 60;

        // Use padStart to ensure double digits for hours and minutes
        const formattedDays = days > 0 ? `${days} day${days > 1 ? 's' : ''} ` : '';
        const formattedHours = hours > 0 ? `${hours} hour${hours > 1 ? 's' : ''} ` : '';
        const formattedMinutes = minutes > 0 ? `${minutes} minute${minutes > 1 ? 's' : ''}` : '';

        return `${formattedDays}${formattedHours}${formattedMinutes}`.trim();
    }

    public static formatDateToDateTimeLocal(date: Date) {
        const padNum = (n: number) => String(n).padStart(2, '0');

        const year = date.getFullYear();
        const month = padNum(date.getMonth() + 1); // Months are 0-based in JavaScript
        const day = padNum(date.getDate());
        const hours = padNum(date.getHours());
        const minutes = padNum(date.getMinutes());

        return `${year}-${month}-${day}T${hours}:${minutes}`;
    }
}