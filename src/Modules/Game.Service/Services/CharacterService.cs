using Game.Service.Request;
using Game.Service.Data;
using Game.Service.Data.Models;
using Game.Service.View;
using Game.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Game.Service.Services;

/// <summary>
/// 角色管理服務
/// </summary>
public class CharacterService(TrpgDbContext context) : ICharacterService
{
	public async Task<List<PlayerCharacterView>> GetAllCharactersAsync(bool isTemplate = false, CancellationToken cancellationToken = default)
	{
		return await context.PlayerCharacters
			.Include(p => p.CharacterAttributes)
				.ThenInclude(a => a.Attribute)
			.Include(p => p.CharacterSkills)
				.ThenInclude(s => s.Skill)
			.Include(p => p.CharacterItems)
				.ThenInclude(i => i.Item)
			.Select(p => new PlayerCharacterView
			{
				Id = p.Id,
				Name = p.Name,
				Gender = p.Gender,
				Age = p.Age,
				PhysicalDesc = p.PhysicalDesc,
				Biography = p.Biography,
				StatusEffects = p.StatusEffects,
				Notes = p.Notes,
				IsDead = p.IsDead,
				LastKnownSceneId = p.LastKnownSceneId,
				IsTemplate = p.IsTemplate,
				IsActive = p.IsActive,
				CreatedAt =  p.CreatedAt,
				UpdatedAt = p.UpdatedAt,
				CharacterAttributes = p.CharacterAttributes
					.Select(a => (CharacterAttributeView?)new CharacterAttributeView
					{
						CharacterId = a.CharacterId,
						AttributeId = a.AttributeId,
						MaxValue = a.MaxValue,
						CurrentValue = a.CurrentValue,
						Attribute = new Attributes
						{
							Id = a.Attribute!.Id,
							Name = a.Attribute.Name,
							Description = a.Attribute.Description
						}
					})
					.ToList(),
				CharacterSkills = p.CharacterSkills
					.Select(s => (CharacterSkillView?)new CharacterSkillView
					{
						CharacterId = s.CharacterId,
						SkillId = s.SkillId,
						Proficiency = s.Proficiency,
						Skill = new SkillView
						{
							Id = s.Skill!.Id,
							Name = s.Skill.Name,
							Description = s.Skill.Description
						}
					})
					.ToList(),
				CharacterItems = p.CharacterItems
					.Select(i => (CharacterItemView?)new CharacterItemView
					{
						CharacterId = i.CharacterId,
						ItemId = i.ItemId,
						Quantity = i.Quantity,
						Item = new ItemView
						{
							Id = i.Item!.Id,
							Name = i.Item.Name,
							Description = i.Item.Description
						}
					})
					.ToList()

			})
			.ToListAsync(cancellationToken);
	}

	public async Task<PlayerCharacterView?> GetCharacterByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		return await context.PlayerCharacters
			.Include(p => p.CharacterAttributes)
				.ThenInclude(a => a.Attribute)
			.Include(p => p.CharacterSkills)
				.ThenInclude(s => s.Skill)
			.Include(p => p.CharacterItems)
				.ThenInclude(i => i.Item)
			.Where(p => p.Id == id)
			.Select(p => new PlayerCharacterView
			{
				Id = p.Id,
				Name = p.Name,
				Gender = p.Gender,
				Age = p.Age,
				PhysicalDesc = p.PhysicalDesc,
				Biography = p.Biography,
				StatusEffects = p.StatusEffects,
				Notes = p.Notes,
				IsDead = p.IsDead,
				LastKnownSceneId = p.LastKnownSceneId,
				IsTemplate = p.IsTemplate,
				IsActive = p.IsActive,
				CreatedAt =  p.CreatedAt,
				UpdatedAt = p.UpdatedAt,
				CharacterAttributes = p.CharacterAttributes
					.Select(a => (CharacterAttributeView?)new CharacterAttributeView
					{
						CharacterId = a.CharacterId,
						AttributeId = a.AttributeId,
						MaxValue = a.MaxValue,
						CurrentValue = a.CurrentValue,
						Attribute = new Attributes
						{
							Id = a.Attribute!.Id,
							Name = a.Attribute.Name,
							Description = a.Attribute.Description
						}
					})
					.ToList(),
				CharacterSkills = p.CharacterSkills
					.Select(s => (CharacterSkillView?)new CharacterSkillView
					{
						CharacterId = s.CharacterId,
						SkillId = s.SkillId,
						Proficiency = s.Proficiency,
						Skill = new SkillView
						{
							Id = s.Skill!.Id,
							Name = s.Skill.Name,
							Description = s.Skill.Description
						}
					})
					.ToList(),
				CharacterItems = p.CharacterItems
					.Select(i => (CharacterItemView?)new CharacterItemView
					{
						CharacterId = i.CharacterId,
						ItemId = i.ItemId,
						Quantity = i.Quantity,
						Item = new ItemView
						{
							Id = i.Item!.Id,
							Name = i.Item.Name,
							Description = i.Item.Description
						}
					})
					.ToList()

			})
			.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<PlayerCharacterView?> CreateCharacterFromTemplateIdAsync(int templateId, CancellationToken cancellationToken = default)
	{
		var template = await context.PlayerCharacters
			.Include(p => p.CharacterAttributes)
			.Include(p => p.CharacterSkills)
			.Include(p => p.CharacterItems)
			.FirstOrDefaultAsync(p => p.Id == templateId, cancellationToken);
		if (template == null) return null;
		var entity = new PlayerCharacter
		{
			Name = template.Name,
			Gender = template.Gender,
			Age = template.Age,
			PhysicalDesc = template.PhysicalDesc,
			Biography = template.Biography,
			StatusEffects = template.StatusEffects,
			Notes = template.Notes,
			IsDead = template.IsDead,
			LastKnownSceneId = template.LastKnownSceneId,
			IsTemplate = false,
			IsActive = template.IsActive,
			CreatedAt = DateTime.UtcNow,
			UpdatedAt = DateTime.UtcNow,
		};
		using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
		try
		{
			context.PlayerCharacters.Add(entity);
			await context.SaveChangesAsync(cancellationToken);

			var newAttrs = template.CharacterAttributes?
				.Select(attr => new CharacterAttribute
				{
					CharacterId = entity.Id,
					AttributeId = attr.AttributeId,
					MaxValue = attr.MaxValue,
					CurrentValue = attr.CurrentValue,
				})
				.ToList();

			if (newAttrs != null && newAttrs.Count > 0)
				context.CharacterAttributes.AddRange(newAttrs);

			var newSkills = template.CharacterSkills?
				.Select(skill => new CharacterSkill
				{
					CharacterId = entity.Id,
					SkillId = skill.SkillId,
					Proficiency = skill.Proficiency,
				})
				.ToList();

			if (newSkills != null && newSkills.Count > 0)
				context.CharacterSkills.AddRange(newSkills);

			var newItems = template.CharacterItems?
				.Select(item => new CharacterItem
				{
					CharacterId = entity.Id,
					ItemId = item.ItemId,
					Quantity = item.Quantity,
				})
				.ToList();

			if (newItems != null && newItems.Count > 0)
				context.CharacterItems.AddRange(newItems);

			await context.SaveChangesAsync(cancellationToken);
			await transaction.CommitAsync(cancellationToken);
		}
		catch
		{
			await transaction.RollbackAsync(cancellationToken);
			return null;
		}

		return await GetCharacterByIdAsync(entity.Id, cancellationToken);
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
			IsDead = character.IsDead,
			LastKnownSceneId = character.LastKnownSceneId,
			IsTemplate = character.IsTemplate,
			IsActive = character.IsActive,
			CreatedAt = DateTime.UtcNow,
			UpdatedAt = DateTime.UtcNow
		};
		context.PlayerCharacters.Add(entity);
		await context.SaveChangesAsync(cancellationToken);
		return entity;
	}

	public async Task<PlayerCharacterView?> UpdateCharacterAsync(int id, PlayerCharacterRequest character, CancellationToken cancellationToken = default)
	{
		var existing = await context.PlayerCharacters.FindAsync(new object[] { id }, cancellationToken);
		if (existing == null) return null;

		existing.Name = character.Name;
		existing.Gender = character.Gender;
		existing.Age = character.Age;
		existing.PhysicalDesc = character.PhysicalDesc;
		existing.Biography = character.Biography;
		existing.StatusEffects = character.StatusEffects;
		existing.Notes = character.Notes;
		existing.IsDead = character.IsDead;
		existing.LastKnownSceneId = character.LastKnownSceneId;
		existing.IsTemplate = character.IsTemplate;
		existing.IsActive = character.IsActive;
		existing.UpdatedAt = DateTime.UtcNow;

		// Collections (skills/items/attributes) should be managed by dedicated methods
		await context.SaveChangesAsync(cancellationToken);
		return await GetCharacterByIdAsync(id, cancellationToken);
	}

	public async Task<PlayerCharacterView?> UpdateCharacterAttributeAsync(int characterId, string attributeName, int newValue, CancellationToken cancellationToken = default)
	{
		var attribute = await context.CharacterAttributes.Include(a => a.Attribute)
			.FirstOrDefaultAsync(a => a.CharacterId == characterId && a.Attribute!.Name == attributeName, cancellationToken);
		if (attribute == null) return null;
		attribute.CurrentValue = newValue;
		await context.SaveChangesAsync(cancellationToken);
		return await GetCharacterByIdAsync(characterId, cancellationToken);
	}

	public async Task<bool> DeleteCharacterAsync(int id, CancellationToken cancellationToken = default)
	{
		var existing = await context.PlayerCharacters.FindAsync(new object[] { id }, cancellationToken);
		if (existing == null) return false;

		context.PlayerCharacters.Remove(existing);
		await context.SaveChangesAsync(cancellationToken);
		return true;
	}
}