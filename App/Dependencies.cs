using App.Middleware;
using Domain.Configuration;
using Domain.Configuration.Options;
using Implementation.Service;
using Implementation.Util;
using Interface.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace App;

public static class Dependencies
{
    public static void RegisterApplicationDependencies(
        this WebApplicationBuilder builder)
    {
        // Configuration
        builder.Configuration.AddJsonFile(
            "secrets.json",
            optional: builder.Environment.IsDevelopment(),
            reloadOnChange: true);

        // Cache
        builder.Services
            .AddMemoryCache()
            .AddScoped<ICacheService, CacheService>();

        // Http
        builder.Services
            .AddHttpContextAccessor()
            .AddScoped<UnhandledExceptionMiddleware>();

        // Auth
        builder.RegisterAuthDependencies();

        if (builder.Environment.IsDevelopment())
        {
            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();

            // Development CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    ApplicationConstants.DevelopmentCorsPolicyName,
                    configurePolicy =>
                    {
                        configurePolicy
                            .WithOrigins(ApplicationConstants.DevelopmentFrontendUrl)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });
        }
    }

    private static void RegisterAuthDependencies(this WebApplicationBuilder builder)
    {
        var jwtOptions = builder.Configuration
            .GetSection(JwtOptions.SectionName)
            .Get<JwtOptions>() ?? throw new NullReferenceException();
        
        builder.Services.AddAuthentication().AddJwtBearer("CookieScheme", options =>
        {
            // Configure JWT settings
            options.TokenValidationParameters = TokenValidationParametersFactory
                .AccessValidationParameters(jwtOptions);

            // Get token from cookie
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies[ApplicationConstants.AccessCookieName];
                    return Task.CompletedTask;
                },
            };
        });
        
        builder.Services.AddAuthorization();
    }
}