using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;
using Game.Service.Interface;
using Game.Service.Request;
using Common.Model;
using Game.Service.View;

namespace ToolBox.Tools.GameTools;

public static partial class TrpgTools
{
	[McpServerTool, Description("Retrieves a list of all player characters.")]
	public static async Task<ResponseBase<List<PlayerCharacterView>>> GetAllCharactersAsync()
	{
		if (_serviceProvider == null) return new ResponseBase<List<PlayerCharacterView>> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
		var characters = await characterService.GetAllCharactersAsync(false);
		return new ResponseBase<List<PlayerCharacterView>> { Success = true, Result = characters };
	}

	[McpServerTool, Description("Creates a new player character from a template ID.")]
	public static async Task<ResponseBase<PlayerCharacterView>> CreateCharacterFromTemplateIdAsync([Description("The template ID user choose to create the character from")] string templateId)
	{
		try{
			if (_serviceProvider == null) return new ResponseBase<PlayerCharacterView> { Success = false, Message = "Service not initialized" };
			using var scope = _serviceProvider.CreateScope();
			var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
			var characters = await characterService.CreateCharacterFromTemplateIdAsync(int.Parse(templateId));
			return characters != null ? new ResponseBase<PlayerCharacterView> { Success = true, Result = characters } : new ResponseBase<PlayerCharacterView> { Success = false, Message = "Create failed" };
		}
		catch(Exception ex)
		{
			return new ResponseBase<PlayerCharacterView> { Success = false, Message = $"Error: {ex.Message}" };
		}
	}

	// [McpServerTool, Description("Create a new player character using CoC7 rules.")]
	// public static async Task<string> CreateCharacterAsync([Description("The player character to create")] PlayerCharacterRequest playerCharacter)
	// {
	// 	if (_serviceProvider == null) return "service not initialized";
	// 	using var scope = _serviceProvider.CreateScope();
	// 	var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
	// 	var checkService = scope.ServiceProvider.GetRequiredService<ICheckService>();
	// 	playerCharacter.IsTemplate = false;
	// 	var result = await characterService.CreateCharacterAsync(playerCharacter);
	// 	return JsonSerializer.Serialize(result);
	// }

	[McpServerTool, Description("Update a player character's information.")]
	public static async Task<ResponseBase<PlayerCharacterView>> UpdateCharacterAsync([Description("The ID of the character to update")] string characterId, [Description("The player character to update")] PlayerCharacterRequest playerCharacter)
	{
		if (_serviceProvider == null) return new ResponseBase<PlayerCharacterView> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
		var character = await characterService.UpdateCharacterAsync(int.Parse(characterId), playerCharacter);
		return character != null ? new ResponseBase<PlayerCharacterView> { Success = true, Result = character } : new ResponseBase<PlayerCharacterView> { Success = false, Message = "Character not found" };
	}

	[McpServerTool, Description("Update a player character's attribute.")]
	public static async Task<ResponseBase<PlayerCharacterView>> UpdateCharacterAttributeAsync([Description("The ID of the character to update")] string characterId, [Description("The name of the attribute to update")] string attributeName, [Description("The new value for the attribute")] string newValue)
	{
		if (_serviceProvider == null) return new ResponseBase<PlayerCharacterView> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
		var character = await characterService.UpdateCharacterAttributeAsync(int.Parse(characterId), attributeName, int.Parse(newValue));
		return character != null ? new ResponseBase<PlayerCharacterView> { Success = true, Result = character } : new ResponseBase<PlayerCharacterView> { Success = false, Message = "Character or attribute not found" };
	}

	[McpServerTool, Description("Get a player character current status.")]
	public static async Task<ResponseBase<PlayerCharacterView>> GetCharacterByIdAsync([Description("The ID of the character to get")] string characterId)
	{
		if (_serviceProvider == null) return new ResponseBase<PlayerCharacterView> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
		var character = await characterService.GetCharacterByIdAsync(int.Parse(characterId));
		return character != null ? new ResponseBase<PlayerCharacterView> { Success = true, Result = character } : new ResponseBase<PlayerCharacterView> { Success = false, Message = "Character not found" };
	}

	[McpServerTool, Description("Delete a player character by ID.")]
	public static async Task<ResponseBase<string>> DeleteCharacterAsync([Description("The ID of the character to delete")] string characterId)
	{
		if (_serviceProvider == null) return new ResponseBase<string> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
		var result = await characterService.DeleteCharacterAsync(int.Parse(characterId));
		return result ? new ResponseBase<string> { Success = true, Result = "Deletion successful" } : new ResponseBase<string> { Success = false, Message = "Character not found" };
	}

	[McpServerTool, Description("Get available character templates for character creation.")]
	public static async Task<ResponseBase<string>> GetAvailableCharacterTemplates()
	{
		if (_serviceProvider == null) return new ResponseBase<string> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
		var characters = await characterService.GetAllCharactersAsync(true);
		return characters != null ? new ResponseBase<string> { Success = true, Result = JsonSerializer.Serialize(characters) } : new ResponseBase<string> { Success = false, Message = "No character templates found" };
	}
}