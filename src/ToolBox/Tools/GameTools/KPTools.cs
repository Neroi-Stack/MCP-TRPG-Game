using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Game.Service.Interface;
using Common.Model;

namespace ToolBox.Tools.GameTools;

public static partial class TrpgTools
{
	[McpServerTool, Description("Generate scene description (KP assist)")]
	public static async Task<ResponseBase<string>> GenerateSceneDescriptionAsync(int sceneId)
	{
		if (_serviceProvider == null) return new ResponseBase<string> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var kp = scope.ServiceProvider.GetRequiredService<IKPService>();
		return new ResponseBase<string> { Success = true, Result = await kp.GenerateSceneDescriptionAsync(sceneId) };
	}
	[McpServerTool, Description("Generate NPC dialogue (KP assist)")]
	public static async Task<ResponseBase<string>> GenerateNpcDialogueAsync(int sceneId)
	{
		if (_serviceProvider == null) return new ResponseBase<string> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var kp = scope.ServiceProvider.GetRequiredService<IKPService>();
		return new ResponseBase<string> { Success = true, Result = await kp.GenerateNpcDialogueAsync(sceneId) };
	}
	[McpServerTool, Description("Suggest checks and difficulties (KP assist)")]
	public static async Task<ResponseBase<string>> SuggestChecksAndDifficultiesAsync(int sceneId)
	{
		if (_serviceProvider == null) return new ResponseBase<string> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var kp = scope.ServiceProvider.GetRequiredService<IKPService>();
		return new ResponseBase<string> { Success = true, Result = await kp.SuggestChecksAndDifficultiesAsync(sceneId) };
	}
	[McpServerTool, Description("Generate random event (KP assist)")]
	public static async Task<ResponseBase<string>> GenerateRandomEventAsync(int sceneId)
	{
		if (_serviceProvider == null) return new ResponseBase<string> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var kp = scope.ServiceProvider.GetRequiredService<IKPService>();
		return new ResponseBase<string> { Success = true, Result = await kp.GenerateRandomEventAsync(sceneId) };
	}
	[McpServerTool, Description("Get game progress suggestions (KP assist)")]
	public static async Task<ResponseBase<string>> GetGameProgressSuggestionsAsync(int scenarioId)
	{
		if (_serviceProvider == null) return new ResponseBase<string> { Success = false, Message = "Service not initialized" };
		using var scope = _serviceProvider.CreateScope();
		var kp = scope.ServiceProvider.GetRequiredService<IKPService>();
		return new ResponseBase<string> { Success = true, Result = await kp.GetGameProgressSuggestionsAsync(scenarioId) };
	}
}