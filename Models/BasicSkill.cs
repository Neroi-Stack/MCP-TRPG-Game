namespace MCPTRPGGame.Models;

/// <summary>
/// 系統基本技能表 - 存儲遊戲中的所有基本技能
/// </summary>
public class BasicSkill
{
    public int Id { get; set; }
    
    /// <summary>
    /// 技能名稱
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// 技能分類
    /// </summary>
    public required string Category { get; set; }
    
    /// <summary>
    /// 基礎成功率
    /// </summary>
    public int BaseSuccessRate { get; set; }
    
    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// 是否為克蘇魯神話技能
    /// </summary>
    public bool IsCthulhuMythos { get; set; } = false;
    
    /// <summary>
    /// 是否為預設技能（所有角色都會有）
    /// </summary>
    public bool IsDefault { get; set; } = true;
    
    /// <summary>
    /// 排序順序
    /// </summary>
    public int DisplayOrder { get; set; }
    
    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}