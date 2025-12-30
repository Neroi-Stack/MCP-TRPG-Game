using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Game.Service.Data;
using ToolBox.GameTools.Tools;
using ModelContextProtocol.Protocol;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Game.Service.Interface;
using Game.Service.Services;
using Common.Interface;
using Common.Services;

// Decide mode by command-line: pass --stdio to run stdio transport, otherwise http
var mode = args.Contains("--stdio") ? "stdio" : "http";

if (mode == "http")
{
	var builder = WebApplication.CreateBuilder(args);

	builder.Logging.AddConsole(consoleLogOptions =>
	{
		consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
	});

	builder.WebHost.UseUrls(builder.Configuration["Hosting:Url"] ?? "http://localhost:5000");

	RegisterCommonServices(builder.Services, builder.Configuration);
	builder.Services.AddScoped<McpContextMiddleware>();
	builder.Services.AddHttpContextAccessor();
	builder.Services.AddMcpServer(_ => { }).WithToolsFromAssembly().WithHttpTransport();

	var app = builder.Build();

	app.UseMiddleware<McpContextMiddleware>();
	app.MapMcp("/mcp");
	InitializeAndSeed(app.Services);

	await app.RunAsync();
}
else
{
	var builder = Host.CreateApplicationBuilder(args);

	builder.Logging.AddConsole(consoleLogOptions =>
	{
		consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
	});

	RegisterCommonServices(builder.Services, builder.Configuration);

	// STDIO-only services
	builder.Services
		.AddMcpServer()
		.WithStdioServerTransport()
		.WithToolsFromAssembly();

	var host = builder.Build();

	InitializeAndSeed(host.Services);

	await host.RunAsync();
}

static void RegisterCommonServices(IServiceCollection services, IConfiguration configuration)
{
	var connectionString = configuration["ConnectionStrings:DefaultConnection"]
		?? "Data Source=trpg.db;";

	services.AddDbContext<TrpgDbContext>(options =>
		options.UseSqlite(connectionString));


	services.AddScoped<DbContext>(sp => sp.GetRequiredService<TrpgDbContext>());

	services.AddScoped<IKPService, KPService>();
	services.AddScoped<ICharacterService, CharacterService>();
	services.AddScoped<ICheckService, CheckService>();
	services.AddScoped<IScenarioService, ScenarioService>();
	services.AddScoped<ISeedDataLoader, SeedDataLoader>();
}

static void InitializeAndSeed(IServiceProvider services)
{
	TrpgTools.Initialize(services);

	using (var scope = services.CreateScope())
	{
		var context = scope.ServiceProvider.GetRequiredService<TrpgDbContext>();

		context.Database.EnsureCreated();
		try
		{
			var seedLoader = scope.ServiceProvider.GetRequiredService<ISeedDataLoader>();
			seedLoader.LoadAllSeedData();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"[SeedDataLoader] {ex.Message}");
		}
	}
}


// http
public class McpContextMiddleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		if (context.Request.HasJsonContentType())
		{
			context.Request.EnableBuffering();

			var mcpRequest = await JsonSerializer.DeserializeAsync<JsonRpcRequest>(context.Request.Body, options: null, context.RequestAborted);

			context.Request.Body.Seek(0, SeekOrigin.Begin);
		}

		await next.Invoke(context);
	}
}