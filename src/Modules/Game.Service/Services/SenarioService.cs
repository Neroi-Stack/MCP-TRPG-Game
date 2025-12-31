using Game.Service.Data;
using Game.Service.View;
using Game.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Game.Service.Services;

/// <summary>
/// 劇本管理服務
/// </summary>
public class ScenarioService : IScenarioService
{
	private readonly TrpgDbContext _context;

	public ScenarioService(TrpgDbContext context)
	{
		_context = context;
	}

	public async Task<List<ScenarioView?>> GetAllScenariosAsync()
	{
		var scenarios = await _context.Scenarios
			.Include(s => s.Scenes)
			.ToListAsync();
		return [.. scenarios.Select(s => (ScenarioView?)s)];
	}

	public async Task<ScenarioView?> GetScenarioByIdAsync(int scenarioId)
	{
		var scenario = await _context.Scenarios
			.Include(x => x.Scenes)
			.FirstOrDefaultAsync(s => s.Id == scenarioId);
		return scenario == null ? null : (ScenarioView)scenario!;
	}
}