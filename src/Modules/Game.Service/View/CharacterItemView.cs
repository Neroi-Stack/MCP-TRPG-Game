
using Game.Service.Data.Models;

namespace Game.Service.View.DTO
{
	public class CharacterItemView
	{
		public int CharacterId { get; set; }
		public int ItemId { get; set; }
		public int Quantity { get; set; }
		public ItemView? Item { get; set; }

		public static implicit operator CharacterItemView?(CharacterItem? character)
		{
			if (character == null) return null;
			return new CharacterItemView
			{
				CharacterId = character.CharacterId,
				ItemId = character.ItemId,
				Quantity = character.Quantity,
				Item = new ItemView
				{
					Id = character.Item!.Id,
					Name = character.Item.Name,
					Category = character.Item.Category,
					Description = character.Item.Description,
					Stats = character.Item.Stats,
					OwnerNotes = character.Item.OwnerNotes,
					Weight = character.Item.Weight,
					IsConsumable = character.Item.IsConsumable,
					IsCursed = character.Item.IsCursed,
					IsActive = character.Item.IsActive,
					DisplayOrder = character.Item.DisplayOrder,
					CreatedAt = character.Item.CreatedAt,
					UpdatedAt = character.Item.UpdatedAt
				}
			};
		}
	}
}