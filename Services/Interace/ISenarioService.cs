using MCPTRPGGame.Data.Models;
using MCPTRPGGame.DTO;

namespace MCPTRPGGame.Services.Interface;

/// <summary>
/// 劇本管理服務
/// </summary>
public interface IScenarioService
{
	public Task<List<ScenarioView?>> GetAllScenariosAsync();
	public Task<ScenarioView?> GetScenarioByIdAsync(int scenarioId);
}