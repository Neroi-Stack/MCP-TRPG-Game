namespace MCPTRPGGame.Models;

/// <summary>
/// 系統隨機元素表 - 存儲各種隨機生成的角色背景元素
/// </summary>
public class RandomElement
{
    public int Id { get; set; }
    
    /// <summary>
    /// 元素類型 (Name, Birthplace, ImportantPerson, Ideology, SignificantLocation, TreasuredPossession)
    /// </summary>
    public required string Type { get; set; }
    
    /// <summary>
    /// 元素描述/內容
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// 權重 (用於隨機選擇時的機率)
    /// </summary>
    public int Weight { get; set; } = 1;
    
    /// <summary>
    /// 文化/地區標籤 (如: Western, Eastern, Modern, Classic)
    /// </summary>
    public string? CultureTag { get; set; }
    
    /// <summary>
    /// 性別限制 (M: 男性, F: 女性, null: 不限)
    /// </summary>
    public string? GenderRestriction { get; set; }
    
    /// <summary>
    /// 職業適用性標籤 (可用逗號分隔多個職業)
    /// </summary>
    public string? OccupationTags { get; set; }
    
    /// <summary>
    /// 年齡範圍適用性 (如: Young, Adult, Elder)
    /// </summary>
    public string? AgeGroup { get; set; }
    
    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// 排序順序
    /// </summary>
    public int DisplayOrder { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}