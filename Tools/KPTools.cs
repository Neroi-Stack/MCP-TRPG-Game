using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using System.ComponentModel;
using MCPTRPGGame.Services.Interface;

namespace MCPTRPGGame.Tools;

public static partial class TrpgTools
{
	[McpServerTool, Description("Generate scene description (KP assist)")]
	public static async Task<string> GenerateSceneDescriptionAsync(int sceneId)
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var kp = scope.ServiceProvider.GetRequiredService<IKPService>();
		return await kp.GenerateSceneDescriptionAsync(sceneId);
	}
	[McpServerTool, Description("Generate NPC dialogue (KP assist)")]
	public static async Task<string> GenerateNpcDialogueAsync(int sceneId)
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var kp = scope.ServiceProvider.GetRequiredService<IKPService>();
		return await kp.GenerateNpcDialogueAsync(sceneId);
	}
	[McpServerTool, Description("Suggest checks and difficulties (KP assist)")]
	public static async Task<string> SuggestChecksAndDifficultiesAsync(int sceneId)
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var kp = scope.ServiceProvider.GetRequiredService<IKPService>();
		return await kp.SuggestChecksAndDifficultiesAsync(sceneId);
	}
	[McpServerTool, Description("Generate random event (KP assist)")]
	public static async Task<string> GenerateRandomEventAsync(int sceneId)
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var kp = scope.ServiceProvider.GetRequiredService<IKPService>();
		return await kp.GenerateRandomEventAsync(sceneId);
	}
	[McpServerTool, Description("Get game progress suggestions (KP assist)")]
	public static async Task<string> GetGameProgressSuggestionsAsync(int scenarioId)
	{
		if (_serviceProvider == null) return "service not initialized";
		using var scope = _serviceProvider.CreateScope();
		var kp = scope.ServiceProvider.GetRequiredService<IKPService>();
		return await kp.GetGameProgressSuggestionsAsync(scenarioId);
	}
}