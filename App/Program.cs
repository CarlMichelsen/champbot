using App;
using App.Middleware;
using Domain.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterApplicationDependencies();

var app = builder.Build();

app.UseMiddleware<UnhandledExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(ApplicationConstants.DevelopmentCorsPolicyName);
}

app.UseAuthentication();

app.UseAuthorization();

var apiGroup = app.MapGroup("api/v1").RequireAuthorization();

app.Run();