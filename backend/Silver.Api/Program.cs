using Silver.Api.Hubs;
using Silver.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigin = Environment.GetEnvironmentVariable("FRONTEND_ORIGIN")
    ?? "http://localhost:3000";

builder.Services.AddCors(options =>
{
    options.AddPolicy("SilverFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddSingleton<RoomService>();
builder.Services.AddSingleton<IGameStateStore, InMemoryGameStateStore>();
builder.Services.AddSingleton<GameSessionService>();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
});

var app = builder.Build();

app.UseCors("SilverFrontend");

app.MapGet("/health", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow }));

app.MapHub<GameHub>("/hubs/game");

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();