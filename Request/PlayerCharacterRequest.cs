using System.ComponentModel;
using MCPTRPGGame.Data.Models;

namespace MCP_TRPG_Game.Request
{
	public class PlayerCharacterRequest
	{
		[Description("name of the character")]
		public string Name { get; set; } = string.Empty;
		[Description("gender of the character")]
		public string Gender { get; set; } = string.Empty;
		[Description("age of the character")]
		public int Age { get; set; }
		[Description("physical description of the character")]
		public string PhysicalDesc { get; set; } = string.Empty;
		[Description("biography of the character")]
		public string Biography { get; set; } = string.Empty;
		[Description("status effects applied to the character")]
		public string StatusEffects { get; set; } = string.Empty;
		[Description("notes about the character")]
		public string Notes { get; set; } = string.Empty;
		[Description("identifier of the character template")]
		public int? CharacterTemplateId { get; set; }
		[Description("indicates if the character is dead")]
		public bool IsDead { get; set; }
		[Description("last known scene ID of the character")]
		public int? LastKnownSceneId { get; set; }
		[Description("indicates if the character is a template")]
		public bool IsTemplate { get; set; }
		[Description("indicates if the character is active")]
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
		public List<CharacterSkill> CharacterSkills { get; set; } = new List<CharacterSkill>();
		public List<CharacterItem> CharacterItems { get; set; } = new List<CharacterItem>();
		public List<CharacterAttribute> CharacterAttributes { get; set; } = new List<CharacterAttribute>();
		public List<CharacterActionSuggestion> CharacterActionSuggestions { get; set; } = new List<CharacterActionSuggestion>();
	}
}