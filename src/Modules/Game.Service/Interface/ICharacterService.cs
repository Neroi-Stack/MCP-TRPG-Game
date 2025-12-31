using Game.Service.Request;
using Game.Service.View.DTO;

namespace Game.Service.Interface;
public interface ICharacterService
{
    Task<List<PlayerCharacterView>> GetAllCharactersAsync(bool isTemplate = false, CancellationToken cancellationToken = default);
    Task<PlayerCharacterView?> GetCharacterByIdAsync(int id, CancellationToken cancellationToken = default);
	Task<PlayerCharacterView?> CreateCharacterFromTemplateIdAsync(int templateId, CancellationToken cancellationToken = default);
    Task<PlayerCharacterView?> CreateCharacterAsync(PlayerCharacterRequest character, CancellationToken cancellationToken = default);
   	Task<PlayerCharacterView?> UpdateCharacterAsync(int id, PlayerCharacterRequest character, CancellationToken cancellationToken = default);
    Task<PlayerCharacterView?> UpdateCharacterAttributeAsync(int characterId, string attributeName, int newValue, CancellationToken cancellationToken = default);
    Task<bool> DeleteCharacterAsync(int id, CancellationToken cancellationToken = default);
}
