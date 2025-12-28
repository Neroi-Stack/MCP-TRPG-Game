namespace MCPTRPGGame.Data.Models
{
	public class CharacterSkill
	{
		public int CharacterId { get; set; }
		public int SkillId { get; set; }
		public int Proficiency { get; set; }

		public PlayerCharacter? PlayerCharacter { get; set; }
		public NonPlayerCharacter? NonPlayerCharacter { get; set; }
		public Skill? Skill { get; set; }
	}
}
