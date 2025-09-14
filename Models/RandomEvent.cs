using System.ComponentModel.DataAnnotations;

namespace MCPTRPGGame.Models
{
    /// <summary>
    /// 隨機事件模型
    /// </summary>
    public class RandomEvent
    {
        public int Id { get; set; }
        
        /// <summary>
        /// 場景類型 (室內/室外/地下室等)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string SceneType { get; set; } = string.Empty;
        
        /// <summary>
        /// 事件描述
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// 事件類別 (聲音/視覺/感覺/環境等)
        /// </summary>
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;
        
        /// <summary>
        /// 最低危險等級要求
        /// </summary>
        public int MinDangerLevel { get; set; } = 1;
        
        /// <summary>
        /// 最高危險等級適用
        /// </summary>
        public int MaxDangerLevel { get; set; } = 10;
        
        /// <summary>
        /// 建議的SAN檢定觸發條件
        /// </summary>
        [StringLength(200)]
        public string? SuggestedSanityCheck { get; set; }
        
        /// <summary>
        /// 建議的技能檢定
        /// </summary>
        [StringLength(200)]
        public string? SuggestedSkillCheck { get; set; }
        
        /// <summary>
        /// KP提示
        /// </summary>
        [StringLength(300)]
        public string? KeeperTips { get; set; }
        
        /// <summary>
        /// 權重 (影響隨機選擇機率)
        /// </summary>
        public int Weight { get; set; } = 1;
        
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
        public DateTime? UpdatedAt { get; set; }
        
        // 可選的場景關聯
        /// <summary>
        /// 特定場景ID (可選，null表示適用於所有該類型場景)
        /// </summary>
        public int? SpecificSceneId { get; set; }
        
        /// <summary>
        /// 關聯的場景
        /// </summary>
        public Scene? SpecificScene { get; set; }
    }
}