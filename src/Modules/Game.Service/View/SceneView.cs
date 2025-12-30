using Game.Service.Data.Models;

namespace MCPTRPGGame.DTO
{
	public class SceneView
	{
		public int Id { get; set; }
		public int ScenarioId { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Background { get; set; } = string.Empty;
		public int OrderInScenario { get; set; }
		public string OpeningNarrative { get; set; } = string.Empty;
		public string KeeperNotes { get; set; } = string.Empty;
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int? CheckRequirementId { get; set; }

		public static implicit operator SceneView?(Scene? scene)
		{
			if (scene == null) return null;
			return new SceneView
			{
				Id = scene.Id,
				ScenarioId = scene.ScenarioId,
				Name = scene.Name,
				Description = scene.Description,
				Background = scene.Background,
				OrderInScenario = scene.OrderInScenario,
				OpeningNarrative = scene.OpeningNarrative,
				KeeperNotes = scene.KeeperNotes,
				IsActive = scene.IsActive,
				CreatedAt = scene.CreatedAt,
				UpdatedAt = scene.UpdatedAt,
				CheckRequirementId = scene.CheckRequirementId
			};
		}
	}
}