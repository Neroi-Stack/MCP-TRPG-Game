namespace MCPTRPGGame.Services.Interface;

/// <summary>
/// 檢定管理服務
/// </summary>
public interface ICheckService
{
	Task<string> SanityCheckAsync(int characterId, string rollExpression);
	Task<string> SkillCheckAsync(int characterId, string skillIdentifier, string rollExpression);
	Task<string> AttributeCheckAsync(int characterId, string attributeIdentifier, string rollExpression);
	Task<string> SavingThrowAsync(int characterId, string throwType, string rollExpression);
	Task<string> CalculateDamageAsync(int characterId, string weaponIdentifier, string rollExpression);
	Task<int> RollDiceAsync(string diceExpression);
	Task<string> AutoRollPlayerAttributeAsync(int characterId);
}