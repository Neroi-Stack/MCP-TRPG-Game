namespace Game.Service.Data.Models
{
	public class SceneRollSuggestionSkill
	{
		public int SceneRollSuggestionId { get; set; }
		public int SkillId { get; set; }

		public SceneRollSuggestion? SceneRollSuggestion { get; set; }
		public Skill? Skill { get; set; }
	}
}
