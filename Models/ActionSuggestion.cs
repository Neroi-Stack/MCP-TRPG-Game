using System.ComponentModel.DataAnnotations;

namespace MCPTRPGGame.Models;

/// <summary>
/// 玩家行動建議模型 - 儲存針對特定玩家行動的檢定建議
/// </summary>
public class ActionSuggestion
{
    public int Id { get; set; }
    
    /// <summary>
    /// 行動關鍵字 (搜索、交談、調查、戰鬥、潛行等)
    /// </summary>
    [Required]
    [StringLength(50)]
    public required string ActionKeyword { get; set; }
    
    /// <summary>
    /// 建議的檢定描述
    /// </summary>
    [Required]
    [StringLength(200)]
    public required string SuggestionDescription { get; set; }
    
    /// <summary>
    /// 顯示順序
    /// </summary>
    public int DisplayOrder { get; set; }
    
    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// 更新時間
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}