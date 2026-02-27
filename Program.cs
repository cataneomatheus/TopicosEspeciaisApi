using TopicosEspeciaisApi.Models;
using TopicosEspeciaisApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Porta configurável via variável de ambiente PORT (Render)
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PlayerService como Singleton (in-memory)
builder.Services.AddSingleton<PlayerService>();

// CORS
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "*";
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (frontendUrl == "*")
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins(frontendUrl.Split(','))
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

var app = builder.Build();

// Swagger sempre habilitado
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

// ── Endpoints ───────────────────────────────────────────

app.MapGet("/api/players", (PlayerService svc) => Results.Ok(svc.GetAll()))
   .WithName("GetAllPlayers");

app.MapGet("/api/players/{id:guid}", (Guid id, PlayerService svc) =>
{
    var player = svc.GetById(id);
    return player is not null ? Results.Ok(player) : Results.NotFound();
})
.WithName("GetPlayerById");

app.MapPost("/api/players", (Player player, PlayerService svc) =>
{
    var created = svc.Add(player);
    return Results.Created($"/api/players/{created.Id}", created);
})
.WithName("CreatePlayer");

app.MapPut("/api/players/{id:guid}", (Guid id, Player updated, PlayerService svc) =>
{
    var player = svc.Update(id, updated);
    return player is not null ? Results.Ok(player) : Results.NotFound();
})
.WithName("UpdatePlayer");

app.MapDelete("/api/players/{id:guid}", (Guid id, PlayerService svc) =>
{
    return svc.Delete(id) ? Results.NoContent() : Results.NotFound();
})
.WithName("DeletePlayer");

app.Run();
