namespace Game.Service.Data.Models
{
	public class NonPlayerCharacter
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Gender { get; set; } = string.Empty;
		public int Age { get; set; }
		public string PhysicalDesc { get; set; } = string.Empty;
		public string Background { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
		public string StatusEffects { get; set; } = string.Empty;
		public string Notes { get; set; } = string.Empty;
		public int? CharacterTemplateId { get; set; }
		public bool IsHostile { get; set; }
		public int? LastKnownSceneId { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		// Navigation collections removed to avoid ambiguous FK mappings for join tables
		public ICollection<NpcReaction> NpcReactions { get; set; } = new List<NpcReaction>();
	}
}
