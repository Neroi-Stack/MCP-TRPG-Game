
using MCPTRPGGame.Data.Models;

namespace MCPTRPGGame.DTO
{
	public class CharacterAttributeView
	{
		public int CharacterId { get; set; }
		public int AttributeId { get; set; }
		public int MaxValue { get; set; }
		public int CurrentValue { get; set; }
		public Attributes? Attribute { get; set; }
		public static implicit operator CharacterAttributeView?(CharacterAttribute? character)
		{
			if (character == null) return null;
			return new CharacterAttributeView
			{
				CharacterId = character.CharacterId,
				AttributeId = character.AttributeId,
				MaxValue = character.MaxValue,
				CurrentValue = character.CurrentValue,
				Attribute = character.Attribute
			};
		}
	}
}