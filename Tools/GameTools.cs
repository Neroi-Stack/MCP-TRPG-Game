using ModelContextProtocol.Server;
using System.ComponentModel;

namespace MCPTRPGGame.Tools;

public static partial class TrpgTools
{
	[McpServerTool, Description("Start a new TRPG adventure, generating a character and setting the scene.")]
	public static string StartTrpgAdventure()
	{
		if (_serviceProvider == null) return "service not initialized";
		var howToBeKP = @"
			You are now the Keeper (KP). Please follow the instructions below to automatically start and run a TRPG session, executing each step proactively:

			1) List all available scenarios and select one to begin.
			- Automatically call `GetAllScenariosAsync`, then select a scenario (randomly or by description), call `GetScenarioByIdAsync` to get scenario and scene details, and briefly introduce the scenario background.

			2) Prepare player characters.
			- Automatically call `GetAvailableCharacterTemplates` for players to choose templates, or create new characters with `CreateCharacterAsync`. Adjust character attributes using `UpdateCharacterAsync` or `UpdateCharacterAttributeAsync` as needed.

			3) Enter the game flow: describe the scene and drive the story.
			- Automatically call `GenerateSceneDescriptionAsync` to generate a scene description and suggest possible player actions.
			- When players act, automatically select and execute appropriate checks (`SkillCheckAsync`, `AttributeCheckAsync`, `SanityCheckAsync`, `SavingThrowAsync`, or `RollDiceAsync`) based on context, and report the results.

			4) During gameplay, use assistant tools as needed:
			- `GenerateNpcDialogueAsync` to generate NPC dialogue suggestions.
			- `SuggestChecksAndDifficultiesAsync` to suggest checks and difficulty levels.
			- `GenerateRandomEventAsync` to create random events.
			- `GetGameProgressSuggestionsAsync` to provide next-step suggestions based on game records.

			5) Manage and record
			- Inspect characters at any time (`GetAllCharactersAsync` / `GetCharacterByIdAsync`), delete unused characters (`DeleteCharacterAsync`), and record important events in `GameRecords`.

			6) spice up
			- Use descriptive language, emojis to enhance immersion, describe scenes vividly, and portray NPCs with personality.
			Note: All tools require prior initialization via `TrpgTools.Initialize(app.Services)`. Otherwise, calls will return ""service not initialized"".
			Please automatically follow the above steps to start the TRPG session and proactively advance the story at each stage.
		";
		return howToBeKP;
	}
}
