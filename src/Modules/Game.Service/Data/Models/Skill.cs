namespace Game.Service.Data.Models
{
	public class Skill
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Category { get; set; } = string.Empty;
		public int BaseSuccessRate { get; set; }
		public string Description { get; set; } = string.Empty;
		public int? ParentSkillId { get; set; }
		public bool IsBasic { get; set; }
		public bool IsActive { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public Skill? ParentSkill { get; set; }
		public ICollection<CharacterSkill> CharacterSkills { get; set; } = new List<CharacterSkill>();
		public ICollection<SceneRollSuggestionSkill> SceneRollSuggestionSkills { get; set; } = new List<SceneRollSuggestionSkill>();
	}
}
