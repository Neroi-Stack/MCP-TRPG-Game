namespace MCPTRPGGame.Data.Models
{
	public class ActionSuggestion
	{
		public int Id { get; set; }
		public string SuggestionType { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
		public double Probability { get; set; }
		public string KeeperNotes { get; set; } = string.Empty;
		public bool IsActive { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int? CheckRequirementId { get; set; }

		public ICollection<SceneActionSuggestion> SceneActionSuggestions { get; set; } = new List<SceneActionSuggestion>();
		public ICollection<CharacterActionSuggestion> CharacterActionSuggestions { get; set; } = new List<CharacterActionSuggestion>();
		public ICollection<ActionSuggestionNpcReaction> ActionSuggestionNpcReactions { get; set; } = new List<ActionSuggestionNpcReaction>();
	}
}