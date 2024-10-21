using App.Endpoints;
using DiscordBot.Hubs;

namespace App;

public static class EndpointExtensions
{
    // IEndpointRouteBuilder apiGroup
    public static void RegisterEndpoints(
        this IEndpointRouteBuilder app)
    {
        var apiGroup = app
            .MapGroup("api/v1")
            .RequireAuthorization();
        
        apiGroup.RegisterEventEndpoints();

        apiGroup.RegisterReminderEndpoints();

        apiGroup.RegisterDiscordEndpoints();

        apiGroup.MapHub<VoiceHub>("voice");
    }
}