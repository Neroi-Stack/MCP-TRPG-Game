using System.ComponentModel.DataAnnotations;

namespace MCPTRPGGame.Models;

/// <summary>
/// 場景檢定建議模型 - 儲存特定場景的檢定建議
/// </summary>
public class SceneRollSuggestion
{
    public int Id { get; set; }
    
    /// <summary>
    /// 場景名稱
    /// </summary>
    [Required]
    [StringLength(100)]
    public required string SceneName { get; set; }
    
    /// <summary>
    /// 檢定建議描述 (例如：【快速交談】- 與酒館老闆交流，獲得布雷克伍德館傳聞)
    /// </summary>
    [Required]
    [StringLength(300)]
    public required string SuggestionDescription { get; set; }
    
    /// <summary>
    /// 建議類型 (skill: 技能檢定, sanity: SAN檢定, attribute: 屬性檢定)
    /// </summary>
    [Required]
    [StringLength(20)]
    public required string SuggestionType { get; set; }
    
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