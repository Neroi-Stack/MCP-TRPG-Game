namespace Game.Service.Data.Models
{
	public class CharacterItem
	{
		public int CharacterId { get; set; }
		public int ItemId { get; set; }
		public int Quantity { get; set; }

		public PlayerCharacter? PlayerCharacter { get; set; }
		public NonPlayerCharacter? NonPlayerCharacter { get; set; }
		public Item? Item { get; set; }
	}
}
