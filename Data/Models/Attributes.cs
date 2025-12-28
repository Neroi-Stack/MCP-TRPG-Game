namespace MCPTRPGGame.Data.Models
{
	public class Attributes
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public ICollection<CharacterAttribute> CharacterAttributes { get; set; } = new List<CharacterAttribute>();
	}
}