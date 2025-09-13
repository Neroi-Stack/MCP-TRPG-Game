using MCPTRPGGame.Data;
using MCPTRPGGame.Models;
using Microsoft.EntityFrameworkCore;

namespace MCPTRPGGame.Services;

/// <summary>
/// 角色管理服務
/// </summary>
public class CharacterService
{
    private readonly TrpgDbContext _context;
    private readonly Coc7RulesService _coc7Rules;

    public CharacterService(TrpgDbContext context, Coc7RulesService coc7Rules)
    {
        _context = context;
        _coc7Rules = coc7Rules;
    }

    /// <summary>
    /// 創建新角色
    /// </summary>
    public async Task<PlayerCharacter> CreateCharacterAsync(string name, string playerName, string occupation = "", int age = 25)
    {
        var character = new PlayerCharacter
        {
            Name = name,
            PlayerName = playerName,
            Occupation = occupation,
            Age = age
        };

        // 使用 CoC7 標準方法生成屬性
        _coc7Rules.GenerateRandomAttributes(character);

        // 應用年齡修正
        _coc7Rules.ApplyAgeModifiers(character);

        // 計算衍生屬性
        _coc7Rules.CalculateDerivedAttributes(character);

        _context.PlayerCharacters.Add(character);
        await _context.SaveChangesAsync();

        // 添加預設技能
        await AddDefaultSkillsAsync(character.Id);

        return character;
    }

    /// <summary>
    /// 獲取角色詳細資料
    /// </summary>
    public async Task<PlayerCharacter?> GetCharacterAsync(int characterId)
    {
        return await _context.PlayerCharacters
            .Include(pc => pc.Skills)
            .ThenInclude(cs => cs.Skill)
            .Include(pc => pc.Items)
            .ThenInclude(ci => ci.Item)
            .FirstOrDefaultAsync(pc => pc.Id == characterId);
    }

    /// <summary>
    /// 更新角色屬性
    /// </summary>
    public async Task<PlayerCharacter> UpdateCharacterAttributesAsync(int characterId,
        int? strength = null, int? dexterity = null, int? power = null, int? constitution = null,
        int? appearance = null, int? education = null, int? size = null, int? intelligence = null)
    {
        var character = await _context.PlayerCharacters.FindAsync(characterId);
        if (character == null)
            throw new ArgumentException("角色不存在", nameof(characterId));

        // 更新屬性值
        if (strength.HasValue) character.Strength = strength.Value;
        if (dexterity.HasValue) character.Dexterity = dexterity.Value;
        if (power.HasValue) character.Power = power.Value;
        if (constitution.HasValue) character.Constitution = constitution.Value;
        if (appearance.HasValue) character.Appearance = appearance.Value;
        if (education.HasValue) character.Education = education.Value;
        if (size.HasValue) character.Size = size.Value;
        if (intelligence.HasValue) character.Intelligence = intelligence.Value;

        // 重新計算衍生屬性
        _coc7Rules.CalculateDerivedAttributes(character);

        character.UpdatedAt = DateTime.UtcNow;
        _context.PlayerCharacters.Update(character);
        await _context.SaveChangesAsync();

        return character;
    }

    /// <summary>
    /// 更新角色生命值
    /// </summary>
    public async Task UpdateCharacterHitPointsAsync(int characterId, int newHitPoints)
    {
        var character = await _context.PlayerCharacters.FindAsync(characterId);
        if (character == null)
            throw new ArgumentException("角色不存在", nameof(characterId));

        var previousHp = character.CurrentHitPoints;
        character.CurrentHitPoints = Math.Max(0, Math.Min(newHitPoints, character.HitPoints));
        character.UpdatedAt = DateTime.UtcNow;

        // 記錄生命值變化
        var log = new CharacterLog
        {
            PlayerCharacterId = characterId,
            LogType = "生命值變化",
            Content = $"生命值從 {previousHp} 變更為 {character.CurrentHitPoints}",
            ValueChange = character.CurrentHitPoints - previousHp,
            PreviousValue = previousHp,
            NewValue = character.CurrentHitPoints
        };

        _context.CharacterLogs.Add(log);
        _context.PlayerCharacters.Update(character);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 更新角色技能點數
    /// </summary>
    public async Task UpdateCharacterSkillAsync(int characterId, string skillName, int professionalPoints = 0, int hobbyPoints = 0, int experiencePoints = 0)
    {
        var character = await _context.PlayerCharacters
            .Include(pc => pc.Skills)
            .ThenInclude(cs => cs.Skill)
            .FirstOrDefaultAsync(pc => pc.Id == characterId);

        if (character == null)
            throw new ArgumentException("角色不存在", nameof(characterId));

        var characterSkill = character.Skills.FirstOrDefault(cs =>
            cs.Skill?.Name.Equals(skillName, StringComparison.OrdinalIgnoreCase) == true);

        if (characterSkill == null)
        {
            // 尋找技能並添加到角色
            var skill = await _context.Skills.FirstOrDefaultAsync(s =>
                s.Name.Equals(skillName, StringComparison.OrdinalIgnoreCase));

            if (skill == null)
                throw new ArgumentException($"技能 '{skillName}' 不存在", nameof(skillName));

            characterSkill = new CharacterSkill
            {
                PlayerCharacterId = characterId,
                SkillId = skill.Id,
                ProfessionalPoints = professionalPoints,
                HobbyPoints = hobbyPoints,
                ExperiencePoints = experiencePoints
            };

            _context.CharacterSkills.Add(characterSkill);
        }
        else
        {
            var previousTotal = characterSkill.TotalPoints;

            characterSkill.ProfessionalPoints = professionalPoints;
            characterSkill.HobbyPoints = hobbyPoints;
            characterSkill.ExperiencePoints = experiencePoints;

            // 記錄技能變化
            var log = new CharacterLog
            {
                PlayerCharacterId = characterId,
                LogType = "技能變化",
                Content = $"{skillName} 技能從 {previousTotal} 變更為 {characterSkill.TotalPoints}",
                ValueChange = characterSkill.TotalPoints - previousTotal,
                PreviousValue = previousTotal,
                NewValue = characterSkill.TotalPoints
            };

            _context.CharacterLogs.Add(log);
            _context.CharacterSkills.Update(characterSkill);
        }

        character.UpdatedAt = DateTime.UtcNow;
        _context.PlayerCharacters.Update(character);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 添加物品到角色
    /// </summary>
    public async Task AddItemToCharacterAsync(int characterId, int itemId, int quantity = 1)
    {
        var character = await _context.PlayerCharacters.FindAsync(characterId);
        var item = await _context.Items.FindAsync(itemId);

        if (character == null)
            throw new ArgumentException("角色不存在", nameof(characterId));
        if (item == null)
            throw new ArgumentException("物品不存在", nameof(itemId));

        // 檢查是否已經有此物品
        var existingItem = await _context.CharacterItems
            .FirstOrDefaultAsync(ci => ci.PlayerCharacterId == characterId && ci.ItemId == itemId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            _context.CharacterItems.Update(existingItem);
        }
        else
        {
            var characterItem = new CharacterItem
            {
                PlayerCharacterId = characterId,
                ItemId = itemId,
                Quantity = quantity
            };
            _context.CharacterItems.Add(characterItem);
        }

        // 記錄物品獲得
        var log = new CharacterLog
        {
            PlayerCharacterId = characterId,
            LogType = "物品獲得",
            Content = $"獲得 {item.Name} x{quantity}"
        };

        _context.CharacterLogs.Add(log);
        character.UpdatedAt = DateTime.UtcNow;
        _context.PlayerCharacters.Update(character);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 獲取所有角色列表
    /// </summary>
    public async Task<List<PlayerCharacter>> GetAllCharactersAsync()
    {
        return await _context.PlayerCharacters
            .OrderByDescending(pc => pc.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// 添加預設技能
    /// </summary>
    private async Task AddDefaultSkillsAsync(int characterId)
    {
        // 從系統基本技能表獲取所有預設技能
        var basicSkills = await _context.BasicSkills
            .Where(bs => bs.IsDefault && bs.IsActive)
            .OrderBy(bs => bs.DisplayOrder)
            .ToListAsync();

        foreach (var basicSkill in basicSkills)
        {
            // 檢查是否已存在對應的技能
            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Name == basicSkill.Name);
            if (skill == null)
            {
                // 根據基本技能資料創建新技能
                skill = new Skill
                {
                    Name = basicSkill.Name,
                    Category = basicSkill.Category,
                    BaseSuccessRate = basicSkill.BaseSuccessRate,
                    IsCthulhuMythos = basicSkill.IsCthulhuMythos
                };
                _context.Skills.Add(skill);
                await _context.SaveChangesAsync();
            }

            // 為角色添加技能
            var characterSkill = new CharacterSkill
            {
                PlayerCharacterId = characterId,
                SkillId = skill.Id,
                ProfessionalPoints = 0,
                HobbyPoints = 0,
                ExperiencePoints = 0
            };

            _context.CharacterSkills.Add(characterSkill);
        }

        await _context.SaveChangesAsync();
    }
}