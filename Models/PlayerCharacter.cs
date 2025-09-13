using System.ComponentModel.DataAnnotations;

namespace MCPTRPGGame.Models;

/// <summary>
/// 玩家角色 (PC) 模型
/// </summary>
public class PlayerCharacter
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 角色名稱
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 玩家名稱
    /// </summary>
    [MaxLength(100)]
    public string PlayerName { get; set; } = string.Empty;

    /// <summary>
    /// 職業
    /// </summary>
    [MaxLength(50)]
    public string Occupation { get; set; } = string.Empty;

    /// <summary>
    /// 年齡
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// 性別
    /// </summary>
    [MaxLength(10)]
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// 出生地
    /// </summary>
    [MaxLength(100)]
    public string Birthplace { get; set; } = string.Empty;

    // 屬性值 (COC 7版規則)
    /// <summary>
    /// 力量 (STR)
    /// </summary>
    public int Strength { get; set; }

    /// <summary>
    /// 敏捷 (DEX)
    /// </summary>
    public int Dexterity { get; set; }

    /// <summary>
    /// 意志 (POW)
    /// </summary>
    public int Power { get; set; }

    /// <summary>
    /// 體質 (CON)
    /// </summary>
    public int Constitution { get; set; }

    /// <summary>
    /// 外貌 (APP)
    /// </summary>
    public int Appearance { get; set; }

    /// <summary>
    /// 教育 (EDU)
    /// </summary>
    public int Education { get; set; }

    /// <summary>
    /// 體型 (SIZ)
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    /// 智力 (INT)
    /// </summary>
    public int Intelligence { get; set; }

    // 衍生屬性
    /// <summary>
    /// 生命值 (HP)
    /// </summary>
    public int HitPoints { get; set; }

    /// <summary>
    /// 當前生命值
    /// </summary>
    public int CurrentHitPoints { get; set; }

    /// <summary>
    /// 魔法值 (MP)
    /// </summary>
    public int MagicPoints { get; set; }

    /// <summary>
    /// 當前魔法值
    /// </summary>
    public int CurrentMagicPoints { get; set; }

    /// <summary>
    /// 理智值 (SAN)
    /// </summary>
    public int Sanity { get; set; }

    /// <summary>
    /// 當前理智值
    /// </summary>
    public int CurrentSanity { get; set; }

    /// <summary>
    /// 幸運值
    /// </summary>
    public int Luck { get; set; }

    /// <summary>
    /// 當前幸運值
    /// </summary>
    public int CurrentLuck { get; set; }

    /// <summary>
    /// 角色背景故事
    /// </summary>
    public string? BackgroundStory { get; set; }

    /// <summary>
    /// 重要之人
    /// </summary>
    [MaxLength(200)]
    public string? ImportantPerson { get; set; }

    /// <summary>
    /// 思想信念
    /// </summary>
    [MaxLength(200)]
    public string? Ideology { get; set; }

    /// <summary>
    /// 重要之地
    /// </summary>
    [MaxLength(200)]
    public string? SignificantLocation { get; set; }

    /// <summary>
    /// 珍貴之物
    /// </summary>
    [MaxLength(200)]
    public string? TreasuredPossession { get; set; }

    /// <summary>
    /// 性格特質
    /// </summary>
    [MaxLength(500)]
    public string? Traits { get; set; }

    /// <summary>
    /// 傷害與疤痕
    /// </summary>
    [MaxLength(500)]
    public string? InjuriesAndScars { get; set; }

    /// <summary>
    /// 恐懼症與狂躁
    /// </summary>
    [MaxLength(500)]
    public string? PhobiasAndManias { get; set; }

    /// <summary>
    /// 角色狀態 (正常、重傷、死亡等)
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
    /// 角色技能
    /// </summary>
    public List<CharacterSkill> Skills { get; set; } = new();

    /// <summary>
    /// 角色物品
    /// </summary>
    public List<CharacterItem> Items { get; set; } = new();

    /// <summary>
    /// 角色記錄
    /// </summary>
    public List<CharacterLog> Logs { get; set; } = new();
}