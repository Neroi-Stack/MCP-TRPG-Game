namespace Game.Service.Data.Models
{
	public class CharacterAttribute
	{
		public int CharacterId { get; set; }
		public int AttributeId { get; set; }
		public int MaxValue { get; set; }
		public int CurrentValue { get; set; }

		public PlayerCharacter? PlayerCharacter { get; set; }
		public NonPlayerCharacter? NonPlayerCharacter { get; set; }
		public Attributes? Attribute { get; set; }
	}
}
