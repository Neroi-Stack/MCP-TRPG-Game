namespace MCPTRPGGame.Data.Models
{
	public class CharacterStatusEffect
	{
		public int CharacterId { get; set; }
		public int StatusEffectId { get; set; }
		public DateTime AppliedAt { get; set; }
		public int RemainingRounds { get; set; }
	}
}