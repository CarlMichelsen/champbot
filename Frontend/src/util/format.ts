export class Format
{
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
        const formattedDays = days > 0 ? `${days} days ` : '';
        const formattedHours = hours > 0 ? `${hours} hours ` : '';
        const formattedMinutes = minutes > 0 ? `${minutes} minutes` : '';

        return `${formattedDays}${formattedHours}${formattedMinutes}`.trim();
    }
}