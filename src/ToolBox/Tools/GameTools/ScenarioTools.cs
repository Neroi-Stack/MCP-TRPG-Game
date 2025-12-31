using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Game.Service.Interface;
using Game.Service.View;
using Common.Model;

namespace ToolBox.Tools.GameTools;

public static partial class TrpgTools
{
	[McpServerTool, Description("Retrieves a list of all available TRPG scenarios for user selection.")]
	public static async Task<ResponseBase<List<ScenarioView?>>> GetAllScenariosAsync()
	{
		if (_serviceProvider == null) return new ResponseBase<List<ScenarioView?>> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var scenarioService = scope.ServiceProvider.GetRequiredService<IScenarioService>();
		var scenarios = await scenarioService.GetAllScenariosAsync();
		return new ResponseBase<List<ScenarioView?>> { Success = true, Result = scenarios };
	}

	[McpServerTool, Description("Fetches detailed information about a specific TRPG scenario based on the provided scenario ID.")]
	public static async Task<ResponseBase<ScenarioView?>> GetScenarioByIdAsync([Description("The ID of the scenario to retrieve information for.")] int scenarioId)
	{
		if (_serviceProvider == null) return new ResponseBase<ScenarioView?> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var scenarioService = scope.ServiceProvider.GetRequiredService<IScenarioService>();
		var scenario = await scenarioService.GetScenarioByIdAsync(scenarioId);
		return new ResponseBase<ScenarioView?> { Success = true, Result = scenario };
	}
}
