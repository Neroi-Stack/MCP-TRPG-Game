using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MCPTRPGGame.Data;
using MCPTRPGGame.Tools;
using MCPTRPGGame.Services;
using MCPTRPGGame.Services.Interface;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConsole(consoleLogOptions =>
{
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"]
    ?? "Data Source=trpg.db;";
builder.Services.AddDbContext<TrpgDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IKPService, KPService>();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<ICheckService, CheckService>();
builder.Services.AddScoped<IScenarioService, ScenarioService>();

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

TrpgTools.Initialize(app.Services);

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TrpgDbContext>();

    context.Database.EnsureCreated();
    try
    {
        var seedLoader = new SeedDataLoader(context);
        seedLoader.LoadAllSeedData();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[SeedDataLoader] {ex.Message}");
    }
}

await app.RunAsync();