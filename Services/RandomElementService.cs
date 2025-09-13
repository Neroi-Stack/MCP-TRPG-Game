using MCPTRPGGame.Data;
using MCPTRPGGame.Models;
using Microsoft.EntityFrameworkCore;

namespace MCPTRPGGame.Services;

/// <summary>
/// 隨機元素服務 - 負責從系統表格中獲取隨機元素
/// </summary>
public class RandomElementService
{
    private readonly TrpgDbContext _context;
    private readonly Random _random = new();

    public RandomElementService(TrpgDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 獲取隨機名稱
    /// </summary>
    public async Task<string> GetRandomNameAsync(string? genderRestriction = null, string? cultureTag = null)
    {
        return await GetRandomElementAsync("Name", genderRestriction, cultureTag) ?? "未知姓名";
    }

    /// <summary>
    /// 獲取隨機出生地
    /// </summary>
    public async Task<string> GetRandomBirthplaceAsync(string? cultureTag = null)
    {
        return await GetRandomElementAsync("Birthplace", null, cultureTag) ?? "未知地區";
    }

    /// <summary>
    /// 獲取隨機重要人物
    /// </summary>
    public async Task<string> GetRandomImportantPersonAsync(string? ageGroup = null)
    {
        return await GetRandomElementAsync("ImportantPerson", null, null, ageGroup) ?? "家人朋友";
    }

    /// <summary>
    /// 獲取隨機信念/價值觀
    /// </summary>
    public async Task<string> GetRandomIdeologyAsync(string? occupationTag = null)
    {
        return await GetRandomElementAsync("Ideology", null, null, null, occupationTag) ?? "追求真理";
    }

    /// <summary>
    /// 獲取隨機重要地點
    /// </summary>
    public async Task<string> GetRandomSignificantLocationAsync(string? ageGroup = null)
    {
        return await GetRandomElementAsync("SignificantLocation", null, null, ageGroup) ?? "童年的家";
    }

    /// <summary>
    /// 獲取隨機珍貴物品
    /// </summary>
    public async Task<string> GetRandomTreasuredPossessionAsync(string? ageGroup = null, string? occupationTag = null)
    {
        return await GetRandomElementAsync("TreasuredPossession", null, null, ageGroup, occupationTag) ?? "家族傳承物品";
    }

    /// <summary>
    /// 通用的隨機元素獲取方法
    /// </summary>
    private async Task<string?> GetRandomElementAsync(string type, string? genderRestriction = null, 
        string? cultureTag = null, string? ageGroup = null, string? occupationTag = null)
    {
        var query = _context.RandomElements
            .Where(re => re.Type == type && re.IsActive);

        // 應用篩選條件
        if (!string.IsNullOrEmpty(genderRestriction))
        {
            query = query.Where(re => re.GenderRestriction == null || re.GenderRestriction == genderRestriction);
        }

        if (!string.IsNullOrEmpty(cultureTag))
        {
            query = query.Where(re => re.CultureTag == null || re.CultureTag == cultureTag);
        }

        if (!string.IsNullOrEmpty(ageGroup))
        {
            query = query.Where(re => re.AgeGroup == null || re.AgeGroup == ageGroup);
        }

        if (!string.IsNullOrEmpty(occupationTag))
        {
            query = query.Where(re => re.OccupationTags == null || 
                re.OccupationTags.Contains(occupationTag));
        }

        var elements = await query.ToListAsync();
        
        if (!elements.Any())
        {
            // 如果沒有符合條件的，返回該類型的任意元素
            elements = await _context.RandomElements
                .Where(re => re.Type == type && re.IsActive)
                .ToListAsync();
        }

        if (!elements.Any())
            return null;

        // 根據權重進行隨機選擇
        var totalWeight = elements.Sum(e => e.Weight);
        var randomValue = _random.Next(1, totalWeight + 1);
        
        var currentWeight = 0;
        foreach (var element in elements)
        {
            currentWeight += element.Weight;
            if (randomValue <= currentWeight)
            {
                return element.Description;
            }
        }

        // 兜底：返回隨機一個
        return elements[_random.Next(elements.Count)].Description;
    }

    /// <summary>
    /// 獲取指定類型的所有元素
    /// </summary>
    public async Task<List<RandomElement>> GetElementsByTypeAsync(string type)
    {
        return await _context.RandomElements
            .Where(re => re.Type == type && re.IsActive)
            .OrderBy(re => re.DisplayOrder)
            .ToListAsync();
    }

    /// <summary>
    /// 添加新的隨機元素
    /// </summary>
    public async Task<RandomElement> AddRandomElementAsync(string type, string description, int weight = 1, 
        string? cultureTag = null, string? genderRestriction = null, string? occupationTags = null, string? ageGroup = null)
    {
        var maxOrder = await _context.RandomElements
            .Where(re => re.Type == type)
            .MaxAsync(re => (int?)re.DisplayOrder) ?? 0;

        var element = new RandomElement
        {
            Type = type,
            Description = description,
            Weight = weight,
            CultureTag = cultureTag,
            GenderRestriction = genderRestriction,
            OccupationTags = occupationTags,
            AgeGroup = ageGroup,
            DisplayOrder = maxOrder + 1
        };

        _context.RandomElements.Add(element);
        await _context.SaveChangesAsync();

        return element;
    }

    /// <summary>
    /// 更新隨機元素
    /// </summary>
    public async Task<RandomElement?> UpdateRandomElementAsync(int id, string? description = null, int? weight = null, 
        string? cultureTag = null, string? genderRestriction = null, string? occupationTags = null, 
        string? ageGroup = null, bool? isActive = null)
    {
        var element = await _context.RandomElements.FindAsync(id);
        if (element == null)
            return null;

        if (description != null) element.Description = description;
        if (weight.HasValue) element.Weight = weight.Value;
        if (cultureTag != null) element.CultureTag = cultureTag;
        if (genderRestriction != null) element.GenderRestriction = genderRestriction;
        if (occupationTags != null) element.OccupationTags = occupationTags;
        if (ageGroup != null) element.AgeGroup = ageGroup;
        if (isActive.HasValue) element.IsActive = isActive.Value;

        element.UpdatedAt = DateTime.UtcNow;
        _context.RandomElements.Update(element);
        await _context.SaveChangesAsync();

        return element;
    }
}