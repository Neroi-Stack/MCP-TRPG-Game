
using Game.Service.Data.Models;

namespace Game.Service.View.DTO
{
	public class CharacterSkillView
	{
		public int CharacterId { get; set; }
		public int SkillId { get; set; }
		public int Proficiency { get; set; }
		public SkillView? Skill { get; set; }

		public static implicit operator CharacterSkillView?(CharacterSkill? character)
		{
			if (character == null) return null;
			return new CharacterSkillView
			{
				CharacterId = character.CharacterId,
				SkillId = character.SkillId,
				Proficiency = character.Proficiency,
				Skill = new SkillView
				{
					Id = character.Skill!.Id,
					Name = character.Skill.Name,
					Description = character.Skill.Description
				}
			};
		}
	}
}