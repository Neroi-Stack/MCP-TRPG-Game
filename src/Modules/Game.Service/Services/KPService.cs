using System.Text;
using Game.Service.Data;
using Game.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Game.Service.Services;

/// <summary>
/// KP輔助管理服務
/// </summary>
public class KPService(TrpgDbContext context) : IKPService
{
	public async Task<string> GenerateSceneDescriptionAsync(int sceneId, CancellationToken cancellationToken = default)
    {
        var scene = await context.Scenes
            .Include(s => s.SceneItems).ThenInclude(si => si.Item)
            .Include(s => s.SceneActionSuggestions).ThenInclude(sas => sas.ActionSuggestion)
            .FirstOrDefaultAsync(s => s.Id == sceneId, cancellationToken);
        if (scene == null) return $"Scene {sceneId} not found.";

        var npcs = await context.NonPlayerCharacters.Where(n => n.LastKnownSceneId == sceneId && n.IsActive)
            .ToListAsync(cancellationToken);

        var items = scene.SceneItems.Select(si => si.Item?.Name ?? "(unknown item)").ToList();
        var actions = scene.SceneActionSuggestions.Select(sas => sas.ActionSuggestion?.Content ?? string.Empty).ToList();

        var sb = new StringBuilder();
        sb.AppendLine($"Scene: {scene.Name}");
        if (!string.IsNullOrWhiteSpace(scene.OpeningNarrative)) sb.AppendLine(scene.OpeningNarrative);
        if (!string.IsNullOrWhiteSpace(scene.Description)) sb.AppendLine(scene.Description);
        if (npcs.Count != 0)
        {
            sb.AppendLine("NPCs present:");
            foreach (var n in npcs) sb.AppendLine($"- {n.Name} ({n.Role})");
        }
        if (items.Count != 0)
        {
            sb.AppendLine("Items in scene:");
            foreach (var it in items) sb.AppendLine($"- {it}");
        }
        if (actions.Count != 0)
        {
            sb.AppendLine("Possible actions:");
            foreach (var a in actions) sb.AppendLine($"- {a}");
        }

        return sb.ToString();
    }

    public async Task<string> GenerateNpcDialogueAsync(int sceneId, CancellationToken cancellationToken = default)
    {
        var npcs = await context.NonPlayerCharacters
            .Include(n => n.NpcReactions)
            .Where(n => n.LastKnownSceneId == sceneId && n.IsActive)
            .ToListAsync(cancellationToken);
        if (npcs.Count == 0) return $"No NPCs found in scene {sceneId}.";

        var sb = new StringBuilder();
        foreach (var npc in npcs)
        {
            sb.AppendLine($"{npc.Name} ({npc.Role}):");
            var reactions = npc.NpcReactions.OrderByDescending(r => r.Probability).Take(3);
            foreach (var r in reactions)
            {
                sb.AppendLine($"- {r.Content} (trigger: {r.Trigger})");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public async Task<string> SuggestChecksAndDifficultiesAsync(int sceneId, CancellationToken cancellationToken = default)
    {
        var suggestions = await context.SceneRollSuggestionScenes
            .Where(s => s.SceneId == sceneId)
            .Select(s => s.SceneRollSuggestion!)
            .Distinct()
            .Include(r => r.SceneRollSuggestionSkills)
            .ToListAsync(cancellationToken);
        if (suggestions.Count == 0) return $"No roll suggestions for scene {sceneId}.";

        var sb = new StringBuilder();
        foreach (var s in suggestions)
        {
            sb.AppendLine($"Suggestion: {s.Content}");
            sb.AppendLine($"Difficulty: {s.Difficulty}");
            if (!string.IsNullOrWhiteSpace(s.KeeperNotes)) sb.AppendLine($"Notes: {s.KeeperNotes}");
            var skills = s.SceneRollSuggestionSkills.Select(sk => sk.Skill?.Name ?? "(unknown)").ToList();
            if (skills.Count != 0) sb.AppendLine($"Related skills: {string.Join(", ", skills)}");
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public async Task<string> GenerateRandomEventAsync(int sceneId, CancellationToken cancellationToken = default)
    {
        var events = await context.RandomEvents.Where(e => e.SceneId == sceneId && e.IsActive).ToListAsync(cancellationToken);
        if (events.Count != 0)
        {
            var chosen = events.OrderByDescending(e => e.EventIntensityId).First();
            return $"Random event: {chosen.Name} - {chosen.Description}";
        }

        // fallback: pick scenario-level or global event
        var scene = await context.Scenes.FindAsync(new object[] { sceneId }, cancellationToken);
        if (scene == null) return $"Scene {sceneId} not found.";
        var fallback = await context.RandomEvents.Where(e => (e.SceneId == null || e.SceneId == sceneId) && e.IsActive)
            .OrderByDescending(e => e.EventIntensityId).FirstOrDefaultAsync(cancellationToken);
        if (fallback == null) return "No available random events.";
        return $"Random event: {fallback.Name} - {fallback.Description}";
    }

    public async Task<string> GetGameProgressSuggestionsAsync(int scenarioId, CancellationToken cancellationToken = default)
    {
        var recent = await context.GameRecords.Where(g => g.ScenarioId == scenarioId)
            .OrderByDescending(g => g.ActionTime).Take(10).ToListAsync(cancellationToken);
        if (recent.Count == 0) return $"No recent game records for scenario {scenarioId}.";

        var sb = new StringBuilder();
        sb.AppendLine("Recent events:");
        foreach (var r in recent)
        {
            sb.AppendLine($"- [{r.ActionTime:yyyy-MM-dd}] {r.Description} (scene:{r.SceneId})");
        }
        sb.AppendLine();
        sb.AppendLine("Suggested next steps for KP:");
        sb.AppendLine("- Follow up on recent events, escalate NPC reactions where appropriate.");
        sb.AppendLine("- Introduce an encounter or clue if progress stalls.");
        return sb.ToString();
    }
}