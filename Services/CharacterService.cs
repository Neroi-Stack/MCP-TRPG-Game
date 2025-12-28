using MCP_TRPG_Game.Request;
using MCPTRPGGame.Data;
using MCPTRPGGame.Data.Models;
using MCPTRPGGame.DTO;
using MCPTRPGGame.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace MCPTRPGGame.Services;

/// <summary>
/// 角色管理服務
/// </summary>
public class CharacterService : ICharacterService
{
	private readonly TrpgDbContext _context;

	public CharacterService(TrpgDbContext context)
	{
		_context = context;
	}

	public async Task<List<PlayerCharacterView?>> GetAllCharactersAsync(bool isTemplate = false, CancellationToken cancellationToken = default)
	{
		return await _context.PlayerCharacters
			.AsNoTracking()
			.Include(p => p.CharacterAttributes)
			.Include(p => p.CharacterSkills)
			.Include(p => p.CharacterItems)
			.Where(p => p.IsTemplate == isTemplate)
			.Select(p => (PlayerCharacterView?)p)
			.ToListAsync(cancellationToken);
	}

	public async Task<PlayerCharacterView?> GetCharacterByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		return await _context.PlayerCharacters
			.Include(p => p.CharacterAttributes)
			.Include(p => p.CharacterSkills)
			.Include(p => p.CharacterItems)
			.FirstOrDefaultAsync(p => p.Id == id && !p.IsTemplate, cancellationToken);
	}

	public async Task<PlayerCharacterView?> CreateCharacterAsync(PlayerCharacterRequest character, CancellationToken cancellationToken = default)
	{
		var entity = new PlayerCharacter
		{
			Name = character.Name,
			Gender = character.Gender,
			Age = character.Age,
			PhysicalDesc = character.PhysicalDesc,
			Biography = character.Biography,
			StatusEffects = character.StatusEffects,
			Notes = character.Notes,
			CharacterTemplateId = character.CharacterTemplateId,
			IsDead = character.IsDead,
			LastKnownSceneId = character.LastKnownSceneId,
			IsTemplate = character.IsTemplate,
			IsActive = character.IsActive,
			CreatedAt = DateTime.UtcNow,
			UpdatedAt = DateTime.UtcNow
		};
		_context.PlayerCharacters.Add(entity);
		await _context.SaveChangesAsync(cancellationToken);
		return entity;
	}

	public async Task<bool> UpdateCharacterAsync(PlayerCharacter character, CancellationToken cancellationToken = default)
	{
		var existing = await _context.PlayerCharacters.FindAsync(new object[] { character.Id }, cancellationToken);
		if (existing == null) return false;

		existing.Name = character.Name;
		existing.Gender = character.Gender;
		existing.Age = character.Age;
		existing.PhysicalDesc = character.PhysicalDesc;
		existing.Biography = character.Biography;
		existing.StatusEffects = character.StatusEffects;
		existing.Notes = character.Notes;
		existing.CharacterTemplateId = character.CharacterTemplateId;
		existing.IsDead = character.IsDead;
		existing.LastKnownSceneId = character.LastKnownSceneId;
		existing.IsTemplate = character.IsTemplate;
		existing.IsActive = character.IsActive;
		existing.UpdatedAt = DateTime.UtcNow;

		// Collections (skills/items/attributes) should be managed by dedicated methods
		await _context.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<bool> UpdateCharacterAttributeAsync(int characterId, string attributeName, int newValue, CancellationToken cancellationToken = default)
	{
		var attribute = await _context.CharacterAttributes.Include(a => a.Attribute)
			.FirstOrDefaultAsync(a => a.CharacterId == characterId && a.Attribute!.Name == attributeName, cancellationToken);
		if (attribute == null) return false;
		attribute.CurrentValue = newValue;
		await _context.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<bool> DeleteCharacterAsync(int id, CancellationToken cancellationToken = default)
	{
		var existing = await _context.PlayerCharacters.FindAsync(new object[] { id }, cancellationToken);
		if (existing == null) return false;

		_context.PlayerCharacters.Remove(existing);
		await _context.SaveChangesAsync(cancellationToken);
		return true;
	}
}