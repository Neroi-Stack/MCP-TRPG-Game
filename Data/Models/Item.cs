namespace MCPTRPGGame.Data.Models
{
	public class Item
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Category { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Stats { get; set; } = string.Empty;
		public string OwnerNotes { get; set; } = string.Empty;
		public decimal Weight { get; set; }
		public bool IsConsumable { get; set; }
		public bool IsCursed { get; set; }
		public bool IsActive { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public ICollection<CharacterItem> CharacterItems { get; set; } = new List<CharacterItem>();
		public ICollection<SceneItem> SceneItems { get; set; } = new List<SceneItem>();
	}
}
