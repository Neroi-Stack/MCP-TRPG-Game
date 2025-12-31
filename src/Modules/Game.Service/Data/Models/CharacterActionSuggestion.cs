namespace Game.Service.Data.Models
{
	public class CharacterActionSuggestion
	{
		public int CharacterId { get; set; }
		public int ActionSuggestionId { get; set; }

		public PlayerCharacter? PlayerCharacter { get; set; }
		public ActionSuggestion? ActionSuggestion { get; set; }
	}
}
