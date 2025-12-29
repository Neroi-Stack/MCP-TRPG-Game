using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using System.ComponentModel;
using MCPTRPGGame.Data.Models;
using System.Text.Json;
using MCPTRPGGame.Services.Interface;
using MCP_TRPG_Game.Request;

namespace MCPTRPGGame.Tools;

public static partial class TrpgTools
{
	[McpServerTool, Description("Retrieves a list of all player characters.")]
	public static async Task<string> GetAllCharactersAsync()
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
		var characters = await characterService.GetAllCharactersAsync(false);
		return JsonSerializer.Serialize(characters);
	}

	[McpServerTool, Description("Creates a new player character from a template ID.")]
	public static async Task<string> CreateCharacterFromTemplateIdAsync([Description("The template ID user choose to create the character from")] string templateId)
	{
		try{
			if (_serviceProvider == null) return "service not initialized";
			using var scope = _serviceProvider.CreateScope();
			var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
			var characters = await characterService.CreateCharacterFromTemplateIdAsync(int.Parse(templateId));
			return characters != null ? $"Create successful : {JsonSerializer.Serialize(characters)}" : "Create failed";
		}
		catch(Exception ex)
		{
			return $"Error: {ex.Message}";
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
	public static async Task<string> UpdateCharacterAsync([Description("The ID of the character to update")] string characterId, [Description("The player character to update")] PlayerCharacterRequest playerCharacter)
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
		var character = await characterService.UpdateCharacterAsync(int.Parse(characterId), playerCharacter);
		return character != null ? $"Update successful : {JsonSerializer.Serialize(character)}" : "Character not found";
	}

	[McpServerTool, Description("Update a player character's attribute.")]
	public static async Task<string> UpdateCharacterAttributeAsync([Description("The ID of the character to update")] string characterId, [Description("The name of the attribute to update")] string attributeName, [Description("The new value for the attribute")] string newValue)
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
		var character = await characterService.UpdateCharacterAttributeAsync(int.Parse(characterId), attributeName, int.Parse(newValue));
		return character != null ? $"Attribute update successful : {JsonSerializer.Serialize(character)}" : "Character or attribute not found";
	}

	[McpServerTool, Description("Get a player character current status.")]
	public static async Task<string> GetCharacterByIdAsync([Description("The ID of the character to get")] string characterId)
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
		var character = await characterService.GetCharacterByIdAsync(int.Parse(characterId));
		return character != null ? JsonSerializer.Serialize(character) : "Character not found";
	}

	[McpServerTool, Description("Delete a player character by ID.")]
	public static async Task<string> DeleteCharacterAsync([Description("The ID of the character to delete")] string characterId)
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
		var result = await characterService.DeleteCharacterAsync(int.Parse(characterId));
		return result ? "Deletion successful" : "Character not found";
	}

	[McpServerTool, Description("Get available character templates for character creation.")]
	public static async Task<string> GetAvailableCharacterTemplates()
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var characterService = scope.ServiceProvider.GetRequiredService<ICharacterService>();
		var characters = await characterService.GetAllCharactersAsync(true);
		return JsonSerializer.Serialize(characters);
	}
}