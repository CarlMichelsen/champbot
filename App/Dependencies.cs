using App.Middleware;
using Database;
using DiscordBot.Accessor;
using DiscordBot.Configuration;
using DiscordBot.Service;
using Domain.Configuration;
using Domain.Configuration.Options;
using Implementation.Accessor;
using Implementation.Handler;
using Implementation.Repository;
using Implementation.Service;
using Implementation.Util;
using Interface.Accessor;
using Interface.Handler;
using Interface.Repository;
using Interface.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
            reloadOnChange: false);
        
        builder.Services
            .Configure<DiscordBotOptions>(builder.Configuration.GetSection(DiscordBotOptions.SectionName));

        // Cache
        builder.Services
            .AddMemoryCache()
            .AddScoped<ICacheService, CacheService>();

        // Accessor
        builder.Services
            .AddSingleton<IDiscordBotAccessor, DiscordBotAccessor>()
            .AddScoped<IUserContextAccessor, UserContextAccessor>();
        
        // Handler
        builder.Services
            .AddScoped<IEventHandler, Implementation.Handler.EventHandler>()
            .AddScoped<IReminderHandler, ReminderHandler>()
            .AddScoped<IDiscordBotHandler, DiscordBotHandler>();

        // Service
        builder.Services
            .AddScoped<IResultErrorLogService, ResultErrorLogService>()
            .AddScoped<IUserDataService, UserDataService>()
            .AddScoped<IEventService, EventService>()
            .AddScoped<IReminderService, ReminderService>()
            .AddScoped<IDiscordBotService, DiscordBotService>();

        // Repository
        builder.Services
            .AddScoped<IEventRepository, EventRepository>()
            .AddScoped<IReminderRepository, ReminderRepository>();

        // BackgroundService
        builder.Services
            .AddHostedService<DiscordBotBackgroundService>();

        // Http
        builder.Services
            .AddHttpContextAccessor()
            .AddScoped<UnhandledExceptionMiddleware>();
        
        // SignalR
        builder.Services
            .AddSignalR();

        // Configure Serilog from "appsettings.(env).json
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.WithProperty("Application", ApplicationConstants.ApplicationName)
            .Enrich.WithProperty("Environment", GetEnvironmentName(builder))
            .CreateLogger();
        builder.Host.UseSerilog();

        // Database
        builder.Services.AddDbContext<DatabaseContext>(
            options =>
            {
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("App"));
                
                if (builder.Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                }
            });

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
                            .WithOrigins(ApplicationConstants.DevelopmentCorsUrl)
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
            .Get<JwtOptions>() ?? throw new NullReferenceException("Failed to get JwtOptions during startup");
        
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

    private static string GetEnvironmentName(WebApplicationBuilder builder) =>
        builder.Environment.IsProduction() ? "Production" : "Development";
}