using MCPTRPGGame.Data;
using MCPTRPGGame.Models;
using Microsoft.EntityFrameworkCore;

namespace MCPTRPGGame.Services;

/// <summary>
/// 劇本管理服務
/// </summary>
public class ScenarioService
{
    private readonly TrpgDbContext _context;

    public ScenarioService(TrpgDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 獲取劇本詳細資料
    /// </summary>
    public async Task<Scenario?> GetScenarioAsync(int scenarioId)
    {
        return await _context.Scenarios
            .Include(s => s.Scenes)
            .FirstOrDefaultAsync(s => s.Id == scenarioId);
    }

    /// <summary>
    /// 獲取劇本的所有NPC
    /// </summary>
    public async Task<List<NonPlayerCharacter>> GetScenarioNpcsAsync(int scenarioId)
    {
        return await _context.NonPlayerCharacters
            .Include(npc => npc.Scene)
            .Where(npc => npc.Scene == null || npc.Scene.ScenarioId == scenarioId)
            .ToListAsync();
    }

    /// <summary>
    /// 創建遊戲會話
    /// </summary>
    public async Task<GameSession> CreateGameSessionAsync(int scenarioId, string sessionName, string keeperName)
    {
        var scenario = await _context.Scenarios.FindAsync(scenarioId);
        if (scenario == null)
            throw new ArgumentException("劇本不存在", nameof(scenarioId));

        var session = new GameSession
        {
            Name = sessionName,
            ScenarioId = scenarioId,
            KeeperName = keeperName,
            Status = "準備中",
            GameTime = "1926年秋天傍晚"
        };

        _context.GameSessions.Add(session);
        await _context.SaveChangesAsync();

        return session;
    }
}