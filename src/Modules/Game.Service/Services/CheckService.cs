using Game.Service.Data;
using Game.Service.Data.Models;
using Game.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Game.Service.Services;

/// <summary>
/// 檢定管理服務
/// </summary>
public class CheckService(TrpgDbContext context) : ICheckService
{
	public async Task<int> RollDiceAsync(string diceExpression)
	{
		var parts = diceExpression.ToLower().Split('d');
		if (parts.Length != 2)
			throw new ArgumentException("Invalid dice expression.");
        if (!int.TryParse(parts[0], out int numDice) || !int.TryParse(parts[1], out int numSides))
            throw new ArgumentException("Invalid dice expression.");
        var random = new Random();
		int total = 0;
		for (int i = 0; i < numDice; i++)
		{
			total += random.Next(1, numSides + 1);
		}
		return total;
	}

	public async Task<string> SanityCheckAsync(int characterId, string rollExpression)
	{
		var roll = await RollDiceAsync(rollExpression);
		var attribute = await context.CharacterAttributes
			.Include(a => a.Attribute)
			.FirstOrDefaultAsync(a => a.CharacterId == characterId && a.Attribute != null && a.Attribute.Name.ToLower().Contains("san"));
		if (attribute == null) return $"No sanity attribute found for character {characterId}. Roll: {roll}";
		var success = roll <= attribute.CurrentValue;
		return $"Roll: {roll}, Threshold: {attribute.CurrentValue}, Success: {success}";
	}

	public async Task<string> AttributeCheckAsync(int characterId, string attributeIdentifier, string rollExpression)
	{
		var attributeQuery = context.CharacterAttributes.Include(a => a.Attribute).Where(a => a.CharacterId == characterId);
		CharacterAttribute? attribute = null;
		if (int.TryParse(attributeIdentifier, out var attrId))
		{
			attribute = await attributeQuery.FirstOrDefaultAsync(a => a.AttributeId == attrId);
		}
		else
		{
			var name = attributeIdentifier.ToLower();
			attribute = await attributeQuery.FirstOrDefaultAsync(a => a.Attribute != null && a.Attribute.Name.ToLower().Contains(name));
		}
		if (attribute == null) return $"Attribute '{attributeIdentifier}' not found for character {characterId}.";
		var roll = await RollDiceAsync(rollExpression);
		var success = roll <= attribute.CurrentValue;
		return $"Roll: {roll}, Threshold: {attribute.CurrentValue}, Success: {success}";
	}

	public async Task<string> SkillCheckAsync(int characterId, string skillIdentifier, string rollExpression)
	{
		var skillQuery = context.CharacterSkills.Include(s => s.Skill).Where(s => s.CharacterId == characterId);
		CharacterSkill? cskill = null;
		if (int.TryParse(skillIdentifier, out var skillId))
		{
			cskill = await skillQuery.FirstOrDefaultAsync(s => s.SkillId == skillId);
		}
		else
		{
			var name = skillIdentifier.ToLower();
			cskill = await skillQuery.FirstOrDefaultAsync(s => s.Skill != null && s.Skill.Name.ToLower().Contains(name));
		}
		if (cskill == null) return $"Skill '{skillIdentifier}' not found for character {characterId}.";
		var baseRate = cskill.Skill?.BaseSuccessRate ?? 0;
		var target = baseRate + cskill.Proficiency;
		var roll = await RollDiceAsync(rollExpression);
		var success = roll <= target;
		return $"Roll: {roll}, Target: {target}, Success: {success}";
	}

	public async Task<string> SavingThrowAsync(int characterId, string throwType, string rollExpression)
	{
		return await AttributeCheckAsync(characterId, throwType, rollExpression);
	}

	public async Task<string> CalculateDamageAsync(int characterId, string weaponIdentifier, string rollExpression)
	{
		Item? item = null;
		if (int.TryParse(weaponIdentifier, out var itemId))
		{
			item = await context.Items.FindAsync(itemId);
		}
		else
		{
			var name = weaponIdentifier.ToLower();
			item = await context.Items.FirstOrDefaultAsync(i => i.Name.ToLower().Contains(name));
		}
		var diceExpr = rollExpression;
		if (item != null && !string.IsNullOrWhiteSpace(item.Stats))
		{
			var stat = item.Stats.Trim();
			if (stat.Contains("d")) diceExpr = stat;
		}
		var total = await RollDiceAsync(diceExpr);
		return $"Damage: {total} (dice: {diceExpr})";
	}

	public async Task<string> AutoRollPlayerAttributeAsync(int characterId)
	{
		var attributeList = await context.Attributes.ToListAsync();

		var characterAttributes = new List<CharacterAttribute>();
		int powValue = 0;

		foreach (var attribute in attributeList)
		{
			int rollResult;
			switch (attribute.Name.ToUpper())
			{
				case "POW":
					rollResult = (await RollDiceAsync("3d6")) * 5;
					powValue = rollResult;
					break;
				case "SAN":
					rollResult = powValue > 0 ? powValue : (await RollDiceAsync("3d6")) * 5;
					break;
				case "SIZ":
				case "INT":
				case "EDU":
					rollResult = (await RollDiceAsync("2d6") + 6) * 5;
					break;
				default:
					rollResult = (await RollDiceAsync("3d6")) * 5;
					break;
			}

			characterAttributes.Add(new CharacterAttribute
			{
				CharacterId = characterId,
				AttributeId = attribute.Id,
				MaxValue = rollResult,
				CurrentValue = rollResult
			});
		}
		context.CharacterAttributes.AddRange(characterAttributes);
		await context.SaveChangesAsync();

		return "Attributes rolled and assigned successfully.";
	}
}