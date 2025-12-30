using Game.Service.Data.Models;

namespace MCPTRPGGame.DTO
{
	public class ItemView
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

		public static implicit operator ItemView?(Item? item)
		{
			if (item == null) return null;
			return new ItemView
			{
				Id = item.Id,
				Name = item.Name,
				Category = item.Category,
				Description = item.Description,
				Stats = item.Stats,
				OwnerNotes = item.OwnerNotes,
				Weight = item.Weight,
				IsConsumable = item.IsConsumable,
				IsCursed = item.IsCursed,
				IsActive = item.IsActive,
				DisplayOrder = item.DisplayOrder,
				CreatedAt = item.CreatedAt,
				UpdatedAt = item.UpdatedAt
			};
		}
	}
}