using MCPTRPGGame.Data.Models;

namespace MCPTRPGGame.DTO
{
	public class PlayerCharacterView
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Gender { get; set; } = string.Empty;
		public int Age { get; set; }
		public string PhysicalDesc { get; set; } = string.Empty;
		public string Biography { get; set; } = string.Empty;
		public string StatusEffects { get; set; } = string.Empty;
		public string Notes { get; set; } = string.Empty;
		public int? CharacterTemplateId { get; set; }
		public bool IsDead { get; set; }
		public int? LastKnownSceneId { get; set; }
		public bool IsTemplate { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public List<CharacterAttributeView?> CharacterAttributes { get; set; } = new();
		public List<CharacterSkillView?> CharacterSkills { get; set; } = new();
		public List<CharacterItemView?> CharacterItems { get; set; } = new();

		public static implicit operator PlayerCharacterView?(PlayerCharacter? character)
		{
			if (character == null) return null;
			return new PlayerCharacterView
			{
				Id = character.Id,
				Name = character.Name,
				Gender = character.Gender,
				Age = character.Age,
				PhysicalDesc = character.PhysicalDesc,
				Biography = character.Biography,
				StatusEffects = character.StatusEffects,
				Notes = character.Notes,
				CharacterTemplateId = character.CharacterTemplateId,
				IsDead = character.IsDead,
				LastKnownSceneId = character.LastKnownSceneId,
				IsTemplate = character.IsTemplate,
				IsActive = character.IsActive,
				CreatedAt = character.CreatedAt,
				UpdatedAt = character.UpdatedAt,
				CharacterAttributes = character.CharacterAttributes.Select(a => (CharacterAttributeView?)a).ToList(),
				CharacterSkills = character.CharacterSkills.Select(s => (CharacterSkillView?)s).ToList(),
				CharacterItems = character.CharacterItems.Select(i => (CharacterItemView?)i).ToList()
			};
		}
	}
}