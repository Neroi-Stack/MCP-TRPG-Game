namespace Game.Service.Data.Models
{
	public class NpcReaction
	{
		public int Id { get; set; }
		public int NonPlayerCharacterId { get; set; }
		public string ReactionType { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
		public string Trigger { get; set; } = string.Empty;
		public double Probability { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int DisplayOrder { get; set; }
		public int? CheckRequirementId { get; set; }

		public NonPlayerCharacter? NonPlayerCharacter { get; set; }
		public ICollection<ActionSuggestionNpcReaction> ActionSuggestionNpcReactions { get; set; } = new List<ActionSuggestionNpcReaction>();
	}
}
