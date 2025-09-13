using System.ComponentModel.DataAnnotations;

namespace MCPTRPGGame.Models;

/// <summary>
/// NPC 模型
/// </summary>
public class NonPlayerCharacter
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// NPC 名稱
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// NPC 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 外觀特徵
    /// </summary>
    public string? Appearance { get; set; }

    /// <summary>
    /// 性格特質
    /// </summary>
    public string? Personality { get; set; }

    /// <summary>
    /// 動機/目標
    /// </summary>
    public string? Motivation { get; set; }

    /// <summary>
    /// 背景故事
    /// </summary>
    public string? Background { get; set; }

    /// <summary>
    /// 可提供的情報
    /// </summary>
    public string? AvailableInformation { get; set; }

    /// <summary>
    /// NPC 類型 (友好、中立、敵對、怪物等)
    /// </summary>
    [MaxLength(20)]
    public string Type { get; set; } = "中立";

    /// <summary>
    /// 所在場景 ID
    /// </summary>
    public int? SceneId { get; set; }

    /// <summary>
    /// 生命值 (如果需要戰鬥)
    /// </summary>
    public int? HitPoints { get; set; }

    /// <summary>
    /// 當前生命值
    /// </summary>
    public int? CurrentHitPoints { get; set; }

    /// <summary>
    /// 護甲值
    /// </summary>
    public int ArmorClass { get; set; } = 0;

    /// <summary>
    /// 傷害加成
    /// </summary>
    [MaxLength(20)]
    public string? DamageBonus { get; set; }

    /// <summary>
    /// 移動力
    /// </summary>
    public int Move { get; set; } = 8;

    /// <summary>
    /// 對抗理智檢定造成的 SAN 消耗
    /// </summary>
    [MaxLength(20)]
    public string? SanityLoss { get; set; }

    /// <summary>
    /// 特殊能力
    /// </summary>
    public string? SpecialAbilities { get; set; }

    /// <summary>
    /// 咒語 (如果有)
    /// </summary>
    public string? Spells { get; set; }

    /// <summary>
    /// 對話選項 (JSON格式)
    /// </summary>
    public string? DialogueOptions { get; set; }

    /// <summary>
    /// 狀態 (活著、死亡、失蹤等)
    /// </summary>
    [MaxLength(20)]
    public string Status { get; set; } = "活著";

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
    /// 所在場景
    /// </summary>
    public Scene? Scene { get; set; }

    /// <summary>
    /// NPC 技能
    /// </summary>
    public List<NpcSkill> Skills { get; set; } = new();
}

/// <summary>
/// NPC 技能關聯模型
/// </summary>
public class NpcSkill
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// NPC ID
    /// </summary>
    public int NonPlayerCharacterId { get; set; }

    /// <summary>
    /// 技能 ID
    /// </summary>
    public int SkillId { get; set; }

    /// <summary>
    /// 技能點數
    /// </summary>
    public int SkillValue { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 導航屬性
    /// <summary>
    /// NPC
    /// </summary>
    public NonPlayerCharacter? NonPlayerCharacter { get; set; }

    /// <summary>
    /// 技能
    /// </summary>
    public Skill? Skill { get; set; }
}