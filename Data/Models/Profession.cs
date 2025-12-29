namespace MCPTRPGGame.Data.Models
{
    public class Profession
    {
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public int BaseSkillPoints { get; set; }
		public ICollection<ProfessionSkill> AllowedSkills { get; set; } = new List<ProfessionSkill>(); // 可選技能
    }
}
