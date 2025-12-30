namespace Game.Service.Data.Models
{
	public class Scene
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

		public Scenario? Scenario { get; set; }
		public ICollection<SceneItem> SceneItems { get; set; } = new List<SceneItem>();
		public ICollection<SceneActionSuggestion> SceneActionSuggestions { get; set; } = new List<SceneActionSuggestion>();
		public ICollection<SceneRollSuggestionScene> SceneRollSuggestionScenes { get; set; } = new List<SceneRollSuggestionScene>();
	}
}
