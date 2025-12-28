
using MCPTRPGGame.Data.Models;

namespace MCPTRPGGame.DTO
{
	public class CharacterItemView
	{
		public int CharacterId { get; set; }
		public int ItemId { get; set; }
		public int Quantity { get; set; }
		public static implicit operator CharacterItemView?(CharacterItem? character)
		{
			if (character == null) return null;
			return new CharacterItemView
			{
				CharacterId = character.CharacterId,
				ItemId = character.ItemId,
				Quantity = character.Quantity
			};
		}
	}
}