using Game.Service.Data.Models;

namespace MCPTRPGGame.DTO
{
	public class AttributeView
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public static implicit operator AttributeView?(Attributes? attribute)
		{
			if (attribute == null) return null;
			return new AttributeView
			{
				Id = attribute.Id,
				Name = attribute.Name,
				Description = attribute.Description
			};
		}
	}
}