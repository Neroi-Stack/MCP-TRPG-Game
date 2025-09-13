using System.ComponentModel.DataAnnotations;

namespace MCPTRPGGame.Models;

/// <summary>
/// 場景/地點模型
/// </summary>
public class Scene
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 場景名稱
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 場景描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 詳細描述 (KP專用)
    /// </summary>
    public string? DetailedDescription { get; set; }

    /// <summary>
    /// 場景類型 (室內、室外、建築物等)
    /// </summary>
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 連接的場景 ID 列表 (JSON格式)
    /// </summary>
    public string? ConnectedScenes { get; set; }

    /// <summary>
    /// 隱藏線索
    /// </summary>
    public string? HiddenClues { get; set; }

    /// <summary>
    /// 可能的檢定 (JSON格式)
    /// </summary>
    public string? PossibleRolls { get; set; }

    /// <summary>
    /// 特殊事件觸發條件
    /// </summary>
    public string? EventTriggers { get; set; }

    /// <summary>
    /// 氛圍描述
    /// </summary>
    public string? Atmosphere { get; set; }

    /// <summary>
    /// 光線條件
    /// </summary>
    [MaxLength(50)]
    public string LightingCondition { get; set; } = "正常";

    /// <summary>
    /// 溫度條件
    /// </summary>
    [MaxLength(50)]
    public string Temperature { get; set; } = "正常";

    /// <summary>
    /// 聲音環境
    /// </summary>
    public string? SoundEnvironment { get; set; }

    /// <summary>
    /// 氣味描述
    /// </summary>
    public string? Smell { get; set; }

    /// <summary>
    /// 危險等級 (1-10)
    /// </summary>
    public int DangerLevel { get; set; } = 1;

    /// <summary>
    /// SAN 檢定觸發條件
    /// </summary>
    public string? SanityCheckTrigger { get; set; }

    /// <summary>
    /// SAN 消耗 (格式: 失敗/成功，例如: "1d4/1d2")
    /// </summary>
    [MaxLength(20)]
    public string? SanityLoss { get; set; }

    /// <summary>
    /// 劇本 ID
    /// </summary>
    public int? ScenarioId { get; set; }

    /// <summary>
    /// 是否已被探索
    /// </summary>
    public bool IsExplored { get; set; } = false;

    /// <summary>
    /// 場景狀態 (正常、封鎖、危險等)
    /// </summary>
    [MaxLength(20)]
    public string Status { get; set; } = "正常";

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 更新時間
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // 導航屬性
    /// <summary>
    /// 場景中的 NPC
    /// </summary>
    public List<NonPlayerCharacter> NPCs { get; set; } = new();

    /// <summary>
    /// 場景中的物品
    /// </summary>
    public List<SceneItem> Items { get; set; } = new();

    /// <summary>
    /// 劇本
    /// </summary>
    public Scenario? Scenario { get; set; }
}

/// <summary>
/// 場景物品關聯模型
/// </summary>
public class SceneItem
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 場景 ID
    /// </summary>
    public int SceneId { get; set; }

    /// <summary>
    /// 物品 ID
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// 物品數量
    /// </summary>
    public int Quantity { get; set; } = 1;

    /// <summary>
    /// 是否隱藏 (需要檢定才能發現)
    /// </summary>
    public bool IsHidden { get; set; } = false;

    /// <summary>
    /// 發現所需檢定
    /// </summary>
    [MaxLength(50)]
    public string? RequiredSkillToFind { get; set; }

    /// <summary>
    /// 發現難度
    /// </summary>
    public int? DifficultyToFind { get; set; }

    /// <summary>
    /// 是否已被發現
    /// </summary>
    public bool IsDiscovered { get; set; } = true;

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 導航屬性
    /// <summary>
    /// 場景
    /// </summary>
    public Scene? Scene { get; set; }

    /// <summary>
    /// 物品
    /// </summary>
    public Item? Item { get; set; }
}