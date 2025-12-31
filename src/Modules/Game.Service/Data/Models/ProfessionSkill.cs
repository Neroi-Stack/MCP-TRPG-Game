namespace Game.Service.Data.Models
{
    public class ProfessionSkill
    {
		public int ProfessionId { get; set; }
		public int SkillId { get; set; }

		public bool IsMandatory { get; set; }

		public Profession? Profession { get; set; }
    }
}
