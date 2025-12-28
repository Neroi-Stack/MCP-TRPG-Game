namespace MCPTRPGGame.Data.Models
{
	public class SceneRollSuggestion
	{
		public int Id { get; set; }
		public string Content { get; set; } = string.Empty;
		public string Difficulty { get; set; } = string.Empty;
		public string KeeperNotes { get; set; } = string.Empty;
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public ICollection<SceneRollSuggestionScene> SceneRollSuggestionScenes { get; set; } = new List<SceneRollSuggestionScene>();
		public ICollection<SceneRollSuggestionSkill> SceneRollSuggestionSkills { get; set; } = new List<SceneRollSuggestionSkill>();
	}
}
