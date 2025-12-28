namespace MCPTRPGGame.Services.Interface;

/// <summary>
/// KP輔助管理服務
/// </summary>
public interface IKPService
{
	Task<string> GenerateSceneDescriptionAsync(int sceneId, CancellationToken cancellationToken = default);
	Task<string> GenerateNpcDialogueAsync(int sceneId, CancellationToken cancellationToken = default);
	Task<string> SuggestChecksAndDifficultiesAsync(int sceneId, CancellationToken cancellationToken = default);
	Task<string> GenerateRandomEventAsync(int sceneId, CancellationToken cancellationToken = default);
	Task<string> GetGameProgressSuggestionsAsync(int scenarioId, CancellationToken cancellationToken = default);
}