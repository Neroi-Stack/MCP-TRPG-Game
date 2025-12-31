namespace Game.Service.Data.Models
{
    public class Profession
    {
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }

		// eg: "EDU*4", "(EDU*2)+(STR+DEX)*2"
		public string? SkillPointFormula { get; set; }
		public ICollection<ProfessionSkill> ProfessionSkills { get; set; } = new List<ProfessionSkill>();
    }
}
