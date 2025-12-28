
using MCPTRPGGame.Data.Models;

namespace MCPTRPGGame.DTO
{
	public class CharacterSkillView
	{
		public int CharacterId { get; set; }
		public int SkillId { get; set; }
		public int Proficiency { get; set; }

		public static implicit operator CharacterSkillView?(CharacterSkill? character)
		{
			if (character == null) return null;
			return new CharacterSkillView
			{
				CharacterId = character.CharacterId,
				SkillId = character.SkillId,
				Proficiency = character.Proficiency
			};
		}
	}
}