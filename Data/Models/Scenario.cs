namespace MCPTRPGGame.Data.Models
{
	public class Scenario
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

		public ICollection<Scene> Scenes { get; set; } = new List<Scene>();
		public ICollection<ScenarioCharacter> ScenarioCharacters { get; set; } = new List<ScenarioCharacter>();
	}
}
