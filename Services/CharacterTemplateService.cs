using MCPTRPGGame.Data;
using MCPTRPGGame.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MCPTRPGGame.Services;

/// <summary>
/// 角色模板服務 - 負責生成預設角色
/// </summary>
public class CharacterTemplateService
{
    private readonly TrpgDbContext _context;
    private readonly Coc7RulesService _coc7Rules;
    private readonly RandomElementService _randomElementService;
    private readonly Random _random = new();

    public CharacterTemplateService(TrpgDbContext context, Coc7RulesService coc7Rules, RandomElementService randomElementService)
    {
        _context = context;
        _coc7Rules = coc7Rules;
        _randomElementService = randomElementService;
    }

    /// <summary>
    /// 獲取所有可用的職業模板
    /// </summary>
    public async Task<List<CharacterTemplate>> GetAvailableTemplatesAsync()
    {
        return await _context.CharacterTemplates
            .Where(t => t.IsDefault)
            .OrderBy(t => t.Occupation)
            .ToListAsync();
    }

    /// <summary>
    /// 根據職業生成隨機角色配置
    /// </summary>
    public async Task<GeneratedCharacterConfig> GenerateCharacterAsync(string occupation)
    {
        var template = await _context.CharacterTemplates
            .FirstOrDefaultAsync(t => t.Occupation == occupation && t.IsDefault);

        if (template == null)
            throw new ArgumentException($"找不到職業模板: {occupation}");

        var attributes = JsonSerializer.Deserialize<Dictionary<string, int>>(template.AttributeTemplate) ?? new();
        var skills = JsonSerializer.Deserialize<Dictionary<string, int>>(template.ProfessionalSkills) ?? new();

        var gender = _random.Next(2) == 0 ? "男" : "女";
        var genderCode = gender == "男" ? "M" : "F";
        var age = GenerateRandomAge(template.AgeRange);
        var ageGroup = GetAgeGroup(age);

        var config = new GeneratedCharacterConfig
        {
            Name = await _randomElementService.GetRandomNameAsync(genderCode, "Western"),
            Occupation = template.Occupation,
            Age = age,
            Gender = gender,
            Birthplace = await _randomElementService.GetRandomBirthplaceAsync("Western"),
            BackgroundStory = template.BackgroundTemplate,
            ImportantPerson = await _randomElementService.GetRandomImportantPersonAsync(ageGroup),
            Ideology = await _randomElementService.GetRandomIdeologyAsync(template.Occupation),
            SignificantLocation = await _randomElementService.GetRandomSignificantLocationAsync(ageGroup),
            TreasuredPossession = await _randomElementService.GetRandomTreasuredPossessionAsync(ageGroup, template.Occupation),
            Traits = template.RecommendedTraits,

            // 屬性值（在模板基礎上加上隨機變化，更符合該職業特性）
            Strength = Math.Max(attributes.GetValueOrDefault("STR", _coc7Rules.Roll3d6() * 5) + _random.Next(-10, 11), 1),
            Dexterity = Math.Max(attributes.GetValueOrDefault("DEX", _coc7Rules.Roll3d6() * 5) + _random.Next(-10, 11), 1),
            Intelligence = Math.Max(attributes.GetValueOrDefault("INT", (_coc7Rules.Roll2d6() + 6) * 5) + _random.Next(-10, 11), 1),
            Constitution = Math.Max(attributes.GetValueOrDefault("CON", _coc7Rules.Roll3d6() * 5) + _random.Next(-10, 11), 1),
            Appearance = Math.Max(attributes.GetValueOrDefault("APP", _coc7Rules.Roll3d6() * 5) + _random.Next(-10, 11), 1),
            Education = Math.Max(attributes.GetValueOrDefault("EDU", (_coc7Rules.Roll2d6() + 6) * 5) + _random.Next(-10, 11), 1),
            Size = Math.Max(attributes.GetValueOrDefault("SIZ", _coc7Rules.Roll3d6() * 5) + _random.Next(-10, 11), 1),
            Power = Math.Max(attributes.GetValueOrDefault("POW", _coc7Rules.Roll3d6() * 5) + _random.Next(-10, 11), 1),

            ProfessionalSkillPoints = skills
        };

        // 應用年齡修正
        _coc7Rules.ApplyAgeModifiersToConfig(config);

        return config;
    }

    /// <summary>
    /// 根據生成的配置創建角色
    /// </summary>
    public async Task<PlayerCharacter> CreateCharacterFromConfigAsync(GeneratedCharacterConfig config, string playerName)
    {
        var character = new PlayerCharacter
        {
            Name = config.Name,
            PlayerName = playerName,
            Occupation = config.Occupation,
            Age = config.Age,
            Gender = config.Gender,
            Birthplace = config.Birthplace,
            BackgroundStory = config.BackgroundStory,
            ImportantPerson = config.ImportantPerson,
            Ideology = config.Ideology,
            SignificantLocation = config.SignificantLocation,
            TreasuredPossession = config.TreasuredPossession,
            Traits = config.Traits,

            // 設置屬性值
            Strength = config.Strength,
            Dexterity = config.Dexterity,
            Intelligence = config.Intelligence,
            Constitution = config.Constitution,
            Appearance = config.Appearance,
            Education = config.Education,
            Size = config.Size,
            Power = config.Power
        };

        // 計算衍生屬性
        _coc7Rules.CalculateDerivedAttributes(character);

        _context.PlayerCharacters.Add(character);
        await _context.SaveChangesAsync();

        // 添加技能（包括專業技能點數）
        await AddSkillsWithProfessionalPointsAsync(character.Id, config.ProfessionalSkillPoints);

        return character;
    }

    private async Task AddSkillsWithProfessionalPointsAsync(int characterId, Dictionary<string, int> professionalSkills)
    {
        // 從系統基本技能表獲取所有預設技能
        var basicSkills = await _context.BasicSkills
            .Where(bs => bs.IsDefault && bs.IsActive)
            .OrderBy(bs => bs.DisplayOrder)
            .ToListAsync();

        foreach (var basicSkill in basicSkills)
        {
            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Name == basicSkill.Name);
            if (skill == null)
            {
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

            var professionalPoints = professionalSkills.GetValueOrDefault(basicSkill.Name, 0);

            var characterSkill = new CharacterSkill
            {
                PlayerCharacterId = characterId,
                SkillId = skill.Id,
                ProfessionalPoints = professionalPoints,
                HobbyPoints = 0,
                ExperiencePoints = 0
            };

            _context.CharacterSkills.Add(characterSkill);
        }

        await _context.SaveChangesAsync();
    }

    // 輔助方法
    private string GetAgeGroup(int age)
    {
        return age switch
        {
            <= 25 => "Young",
            <= 45 => "Adult",
            _ => "Elder"
        };
    }

    private int GenerateRandomAge(string ageRange)
    {
        var parts = ageRange.Split('-');
        if (parts.Length == 2 && int.TryParse(parts[0], out var min) && int.TryParse(parts[1], out var max))
        {
            return _random.Next(min, max + 1);
        }
        return 30;
    }
}