namespace MCPTRPGGame.Data.Models
{
	public class ActionSuggestionNpcReaction
	{
		public int ActionSuggestionId { get; set; }
		public int NpcReactionId { get; set; }

		public ActionSuggestion? ActionSuggestion { get; set; }
		public NpcReaction? NpcReaction { get; set; }
	}
}
