using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Game.Service.Interface;

namespace ToolBox.GameTools.Tools;

public static partial class TrpgTools
{
	[McpServerTool, Description("Perform a sanity check on the system.")]
	public static async Task<string> SanityCheck([Description("Character ID")] string characterId, [Description("Roll expression")] string roll = "1d100")
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var checkService = scope.ServiceProvider.GetRequiredService<ICheckService>();
		return await checkService.SanityCheckAsync(int.Parse(characterId), roll);
	}

	[McpServerTool, Description("Perform a skill check for a character.")]

	public static async Task<string> SkillCheckAsync([Description("Character ID")] string characterId, [Description("Skill ID")] string skillId, [Description("Roll expression")] string roll = "1d100")
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var checkService = scope.ServiceProvider.GetRequiredService<ICheckService>();
		return await checkService.SkillCheckAsync(int.Parse(characterId), skillId, roll);
	}

	[McpServerTool, Description("Perform an attribute check for a character.")]
	public static async Task<string> AttributeCheckAsync([Description("Character ID")] string characterId, [Description("Attribute ID")] string attributeId, [Description("Roll expression")] string roll = "1d100")
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var checkService = scope.ServiceProvider.GetRequiredService<ICheckService>();
		return await checkService.AttributeCheckAsync(int.Parse(characterId), attributeId, roll);
	}

	[McpServerTool, Description("Perform a Saving Throw on the system.")]
	public static async Task<string> SavingThrowAsync([Description("Character ID")] string characterId, [Description("Type of Saving Throw")] string throwType, [Description("Roll expression")] string roll = "1d100")
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var checkService = scope.ServiceProvider.GetRequiredService<ICheckService>();
		return await checkService.SavingThrowAsync(int.Parse(characterId), throwType, roll);
	}

	[McpServerTool, Description("Calculate damage for an attack.")]
	public static async Task<string> CalculateDamageAsync([Description("Character ID")] string characterId, [Description("Weapon ID")] string weaponId, [Description("Roll expression")] string roll = "1d6")
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var checkService = scope.ServiceProvider.GetRequiredService<ICheckService>();
		return await checkService.CalculateDamageAsync(int.Parse(characterId), weaponId, roll);
	}

	[McpServerTool, Description("Roll dice using standard notation.")]
	public static async Task<string> RollDiceAsync([Description("Roll expression")] string rollExpression)
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var checkService = scope.ServiceProvider.GetRequiredService<ICheckService>();
		var result = await checkService.RollDiceAsync(rollExpression);
		return result.ToString();
	}
}
