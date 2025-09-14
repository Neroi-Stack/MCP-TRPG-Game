using System.ComponentModel.DataAnnotations;

namespace MCPTRPGGame.Models;

/// <summary>
/// NPC反應建議模型 - 儲存不同玩家態度下的NPC反應
/// </summary>
public class NpcReaction
{
    public int Id { get; set; }
    
    /// <summary>
    /// NPC名稱
    /// </summary>
    [Required]
    [StringLength(100)]
    public required string NpcName { get; set; }
    
    /// <summary>
    /// 玩家態度類型 (友善、直接、威脅、賄賂、學術、急躁、恭敬、關心、質疑、專業等)
    /// </summary>
    [Required]
    [StringLength(50)]
    public required string PlayerApproach { get; set; }
    
    /// <summary>
    /// NPC的反應描述
    /// </summary>
    [Required]
    [StringLength(500)]
    public required string ReactionDescription { get; set; }
    
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