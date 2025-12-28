using MCP_TRPG_Game.Request;
using MCPTRPGGame.Data.Models;
using MCPTRPGGame.DTO;

namespace MCPTRPGGame.Services.Interface;

public interface ICharacterService
{
    Task<List<PlayerCharacterView?>> GetAllCharactersAsync(bool isTemplate = false, CancellationToken cancellationToken = default);
    Task<PlayerCharacterView?> GetCharacterByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PlayerCharacterView?> CreateCharacterAsync(PlayerCharacterRequest character, CancellationToken cancellationToken = default);
    Task<bool> UpdateCharacterAsync(PlayerCharacter character, CancellationToken cancellationToken = default);
    Task<bool> UpdateCharacterAttributeAsync(int characterId, string attributeName, int newValue, CancellationToken cancellationToken = default);
    Task<bool> DeleteCharacterAsync(int id, CancellationToken cancellationToken = default);
}
