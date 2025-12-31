using Game.Service.Data.Models;

namespace Game.Service.View.DTO
{
	public class ScenarioView
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Background { get; set; } = string.Empty;
		public string OpeningNarrative { get; set; } = string.Empty;
		public int RecommendedPlayerCount { get; set; }
		public int EstimatedDuration { get; set; }
		public string DifficultyLevel { get; set; } = string.Empty;
		public string Tags { get; set; } = string.Empty;
		public string MainPlotPoints { get; set; } = string.Empty;
		public string TruthAndEndings { get; set; } = string.Empty;
		public string KeeperNotes { get; set; } = string.Empty;
		public string DefaultRollsAndSanity { get; set; } = string.Empty;
		public bool IsActive { get; set; } = true;
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public List<SceneView?> Scenes { get; set; } = new();

		public static implicit operator ScenarioView?(Scenario? scenario)
		{
			if (scenario == null) return null;
			return new ScenarioView
			{
				Id = scenario.Id,
				Name = scenario.Name,
				Description = scenario.Description,
				Background = scenario.Background,
				OpeningNarrative = scenario.OpeningNarrative,
				RecommendedPlayerCount = scenario.RecommendedPlayerCount,
				EstimatedDuration = scenario.EstimatedDuration,
				DifficultyLevel = scenario.DifficultyLevel,
				Tags = scenario.Tags,
				MainPlotPoints = scenario.MainPlotPoints,
				TruthAndEndings = scenario.TruthAndEndings,
				KeeperNotes = scenario.KeeperNotes,
				DefaultRollsAndSanity = scenario.DefaultRollsAndSanity,
				IsActive = scenario.IsActive,
				CreatedAt = scenario.CreatedAt,
				UpdatedAt = scenario.UpdatedAt,
				Scenes = scenario.Scenes.Select(s => (SceneView?)s).ToList() ?? new List<SceneView?>()
			};
		}
	}
}