namespace Game.Service.Data.Models
{
	public class SceneActionSuggestion
	{
		public int SceneId { get; set; }
		public int ActionSuggestionId { get; set; }

		public Scene? Scene { get; set; }
		public ActionSuggestion? ActionSuggestion { get; set; }
	}
}
