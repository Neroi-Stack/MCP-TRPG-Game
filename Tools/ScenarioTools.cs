using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;
using MCPTRPGGame.Services.Interface;

namespace MCPTRPGGame.Tools;

public static partial class TrpgTools
{
	[McpServerTool, Description("Retrieves a list of all available TRPG scenarios for user selection.")]
	public static async Task<string> GetAllScenariosAsync()
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
        var scenarioService = scope.ServiceProvider.GetRequiredService<IScenarioService>();
		var scenarios = await scenarioService.GetAllScenariosAsync();
		return JsonSerializer.Serialize(scenarios);
	}

    [McpServerTool, Description("Fetches detailed information about a specific TRPG scenario based on the provided scenario ID.")]
    public static async Task<string> GetScenarioByIdAsync([Description("The ID of the scenario to retrieve information for.")] int scenarioId)
    {
        if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
        var scenarioService = scope.ServiceProvider.GetRequiredService<IScenarioService>();
		var scenario = await scenarioService.GetScenarioByIdAsync(scenarioId);
		return JsonSerializer.Serialize(scenario);
    }
}
