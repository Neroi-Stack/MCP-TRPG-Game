namespace MCPTRPGGame.Data.Models
{
	public class CheckRequirement
	{
		public int Id { get; set; }
		public int? SkillId { get; set; }
		public int? AttributeId { get; set; }
		public string DiceExpression { get; set; } = string.Empty;
		public string Difficulty { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string KeeperNotes { get; set; } = string.Empty;
		public bool IsActive { get; set; } = true;
		public int DisplayOrder { get; set; }
	}	
}