namespace MCPTRPGGame.Data.Models
{
	public class ScenarioCharacter
	{
		public int ScenarioId { get; set; }
		public int CharacterId { get; set; }

		public Scenario? Scenario { get; set; }
		public PlayerCharacter? PlayerCharacter { get; set; }
		public NonPlayerCharacter? NonPlayerCharacter { get; set; }
	}
}
