namespace MCPTRPGGame.Data.Models
{
	public class GameRecords
	{
		public int Id { get; set; }
		public string Description { get; set; } = string.Empty;
		public string RecordType { get; set; } = string.Empty;
		public int? ActorId { get; set; }
		public string ActorType { get; set; } = string.Empty;
		public int? SceneId { get; set; }
		public int? ScenarioId { get; set; }
		public int? RandomEventId { get; set; }
		public DateTime ActionTime { get; set; }
		public string ResultJson { get; set; } = string.Empty;
		public string KeeperNotes { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
	}
}
