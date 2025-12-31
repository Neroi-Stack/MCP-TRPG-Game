namespace Game.Service.Data.Models
{
	public class PlayerCharacter
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Gender { get; set; } = string.Empty;
		public int Age { get; set; }
		public string PhysicalDesc { get; set; } = string.Empty;
		public string Biography { get; set; } = string.Empty;
		public string StatusEffects { get; set; } = string.Empty;
		public string Notes { get; set; } = string.Empty;
		public bool IsDead { get; set; }
		public int? LastKnownSceneId { get; set; }
		public bool IsTemplate { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public ICollection<CharacterSkill> CharacterSkills { get; set; } = new List<CharacterSkill>();
		public ICollection<CharacterItem> CharacterItems { get; set; } = new List<CharacterItem>();
		public ICollection<CharacterAttribute> CharacterAttributes { get; set; } = new List<CharacterAttribute>();
		public ICollection<CharacterActionSuggestion> CharacterActionSuggestions { get; set; } = new List<CharacterActionSuggestion>();
	}
}
