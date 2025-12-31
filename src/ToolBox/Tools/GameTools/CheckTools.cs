using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Game.Service.Interface;
using Common.Model;

namespace ToolBox.Tools.GameTools;

public static partial class TrpgTools
{
	[McpServerTool, Description("Perform a sanity check on the system.")]
	public static async Task<ResponseBase<string>> SanityCheck([Description("Character ID")] string characterId, [Description("Roll expression")] string roll = "1d100")
	{
		if (_serviceProvider == null) return new ResponseBase<string> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var checkService = scope.ServiceProvider.GetRequiredService<ICheckService>();
		return new ResponseBase<string> { Success = true, Result = await checkService.SanityCheckAsync(int.Parse(characterId), roll) };
	}

	[McpServerTool, Description("Perform a skill check for a character.")]

	public static async Task<ResponseBase<string>> SkillCheckAsync([Description("Character ID")] string characterId, [Description("Skill ID")] string skillId, [Description("Roll expression")] string roll = "1d100")
	{
		if (_serviceProvider == null) return new ResponseBase<string> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var checkService = scope.ServiceProvider.GetRequiredService<ICheckService>();
		return new ResponseBase<string> { Success = true, Result = await checkService.SkillCheckAsync(int.Parse(characterId), skillId, roll) };
	}

	[McpServerTool, Description("Perform an attribute check for a character.")]
	public static async Task<ResponseBase<string>> AttributeCheckAsync([Description("Character ID")] string characterId, [Description("Attribute ID")] string attributeId, [Description("Roll expression")] string roll = "1d100")
	{
		if (_serviceProvider == null) return new ResponseBase<string> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var checkService = scope.ServiceProvider.GetRequiredService<ICheckService>();
		return new ResponseBase<string> { Success = true, Result = await checkService.AttributeCheckAsync(int.Parse(characterId), attributeId, roll) };
	}

	[McpServerTool, Description("Perform a Saving Throw on the system.")]
	public static async Task<ResponseBase<string>> SavingThrowAsync([Description("Character ID")] string characterId, [Description("Type of Saving Throw")] string throwType, [Description("Roll expression")] string roll = "1d100")
	{
		if (_serviceProvider == null) return new ResponseBase<string> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var checkService = scope.ServiceProvider.GetRequiredService<ICheckService>();
		return new ResponseBase<string> { Success = true, Result = await checkService.SavingThrowAsync(int.Parse(characterId), throwType, roll) };
	}

	[McpServerTool, Description("Calculate damage for an attack.")]
	public static async Task<ResponseBase<string>> CalculateDamageAsync([Description("Character ID")] string characterId, [Description("Weapon ID")] string weaponId, [Description("Roll expression")] string roll = "1d6")
	{
		if (_serviceProvider == null) return new ResponseBase<string> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var checkService = scope.ServiceProvider.GetRequiredService<ICheckService>();
		return new ResponseBase<string> { Success = true, Result = await checkService.CalculateDamageAsync(int.Parse(characterId), weaponId, roll) };
	}

	[McpServerTool, Description("Roll dice using standard notation.")]
	public static async Task<ResponseBase<int>> RollDiceAsync([Description("Roll expression")] string rollExpression)
	{
		if (_serviceProvider == null) return new ResponseBase<int> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var checkService = scope.ServiceProvider.GetRequiredService<ICheckService>();
		var result = await checkService.RollDiceAsync(rollExpression);
		return new ResponseBase<int> { Success = true, Result = result };
	}
}
