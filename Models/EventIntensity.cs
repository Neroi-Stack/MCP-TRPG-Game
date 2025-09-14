using System.ComponentModel.DataAnnotations;

namespace MCPTRPGGame.Models
{
    /// <summary>
    /// 事件強度等級模型
    /// </summary>
    public class EventIntensity
    {
        public int Id { get; set; }
        
        /// <summary>
        /// 強度鍵值 (MILD/MODERATE/STRONG等)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Key { get; set; } = string.Empty;
        
        /// <summary>
        /// 顯示名稱
        /// </summary>
        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; } = string.Empty;
        
        /// <summary>
        /// 最低危險等級門檻
        /// </summary>
        public int MinDangerLevel { get; set; }
        
        /// <summary>
        /// 最高危險等級門檻
        /// </summary>
        public int MaxDangerLevel { get; set; }
        
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        
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
    }
}