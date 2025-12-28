namespace MCPTRPGGame.Data.Models
{
	public class SceneRollSuggestionScene
	{
		public int SceneRollSuggestionId { get; set; }
		public int SceneId { get; set; }

		public SceneRollSuggestion? SceneRollSuggestion { get; set; }
		public Scene? Scene { get; set; }
	}
}
