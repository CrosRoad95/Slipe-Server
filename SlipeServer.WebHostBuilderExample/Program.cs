using Microsoft.Extensions.DependencyInjection.Extensions;
using SlipeServer.Hosting;
using SlipeServer.Server.Resources.Serving;
using SlipeServer.Server;
using SlipeServer.Server.ServerBuilders;
using SlipeServer.Server.Mappers;
using SlipeServer.Console.Logic;
using SlipeServer.Lua;
using SlipeServer.WebHostBuilderExample;
using SlipeServer.Server.Behaviour;

Directory.SetCurrentDirectory(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()!.Location)!);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration.GetRequiredSection("MtaServer").Get<Configuration>();

builder.Services.AddHttpClient();
builder.Services.AddDefaultMtaServerServices();
builder.Services.AddLua();

builder.Services.AddSingleton<IResourceServer, BasicHttpServer>();

builder.Services.AddHostedService<SampleHostedService>(); // Use instead of logics
builder.Services.TryAddSingleton<ILogger>(x => x.GetRequiredService<ILogger<MtaServer>>());

builder.AddMtaServer(serverBuilder =>
{
    var isDevelopment = builder.Environment.IsDevelopment();
    var exceptBehaviours = ServerBuilderDefaultBehaviours.Default;

    if (isDevelopment)
        exceptBehaviours |= ServerBuilderDefaultBehaviours.MasterServerAnnouncementBehaviour;

    serverBuilder.AddHostedDefaults(exceptBehaviours: exceptBehaviours);
    serverBuilder.AddSampleResource();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
