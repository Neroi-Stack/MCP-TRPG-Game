namespace MCPTRPGGame.Data.Models
{
	public class SceneItem
	{
		public int SceneId { get; set; }
		public int ItemId { get; set; }

		public Scene? Scene { get; set; }
		public Item? Item { get; set; }
	}
}
