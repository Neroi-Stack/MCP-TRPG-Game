using System.ComponentModel.DataAnnotations;

namespace MCPTRPGGame.Models;

/// <summary>
/// 物品模型
/// </summary>
public class Item
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 物品名稱
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 物品描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 物品類型 (武器、護甲、道具、書籍、線索等)
    /// </summary>
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 重量 (公斤)
    /// </summary>
    public decimal Weight { get; set; } = 0;

    /// <summary>
    /// 價值 (美元)
    /// </summary>
    public decimal Value { get; set; } = 0;

    /// <summary>
    /// 是否為線索物品
    /// </summary>
    public bool IsClue { get; set; } = false;

    /// <summary>
    /// 線索資訊 (如果是線索物品)
    /// </summary>
    public string? ClueInformation { get; set; }

    /// <summary>
    /// 是否為武器
    /// </summary>
    public bool IsWeapon { get; set; } = false;

    /// <summary>
    /// 武器傷害 (例如: "1d6+1")
    /// </summary>
    [MaxLength(20)]
    public string? Damage { get; set; }

    /// <summary>
    /// 武器射程
    /// </summary>
    public int? Range { get; set; }

    /// <summary>
    /// 武器攻擊次數/輪
    /// </summary>
    public int? AttacksPerRound { get; set; }

    /// <summary>
    /// 彈匣容量
    /// </summary>
    public int? MagazineCapacity { get; set; }

    /// <summary>
    /// 故障值
    /// </summary>
    public int? Malfunction { get; set; }

    /// <summary>
    /// 是否為護甲
    /// </summary>
    public bool IsArmor { get; set; } = false;

    /// <summary>
    /// 護甲值
    /// </summary>
    public int? ArmorValue { get; set; }

    /// <summary>
    /// 是否為書籍/魔法書
    /// </summary>
    public bool IsBook { get; set; } = false;

    /// <summary>
    /// 閱讀所需時間 (小時)
    /// </summary>
    public int? ReadingTime { get; set; }

    /// <summary>
    /// 克蘇魯神話技能增加
    /// </summary>
    public int? CthulhuMythosGain { get; set; }

    /// <summary>
    /// 理智消耗 (閱讀書籍時)
    /// </summary>
    [MaxLength(20)]
    public string? SanityLoss { get; set; }

    /// <summary>
    /// 咒語內容 (如果書籍包含咒語)
    /// </summary>
    public string? Spells { get; set; }

    /// <summary>
    /// 特殊效果
    /// </summary>
    public string? SpecialEffects { get; set; }

    /// <summary>
    /// 使用條件
    /// </summary>
    public string? UsageConditions { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 導航屬性
    /// <summary>
    /// 角色物品
    /// </summary>
    public List<CharacterItem> CharacterItems { get; set; } = new();

    /// <summary>
    /// 場景物品
    /// </summary>
    public List<SceneItem> SceneItems { get; set; } = new();
}

/// <summary>
/// 角色物品關聯模型
/// </summary>
public class CharacterItem
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 角色 ID
    /// </summary>
    public int PlayerCharacterId { get; set; }

    /// <summary>
    /// 物品 ID
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// 物品數量
    /// </summary>
    public int Quantity { get; set; } = 1;

    /// <summary>
    /// 是否已裝備
    /// </summary>
    public bool IsEquipped { get; set; } = false;

    /// <summary>
    /// 物品當前狀態 (完好、損壞、故障等)
    /// </summary>
    [MaxLength(20)]
    public string Condition { get; set; } = "完好";

    /// <summary>
    /// 當前彈藥數 (如果是武器)
    /// </summary>
    public int? CurrentAmmo { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// 獲得時間
    /// </summary>
    public DateTime AcquiredAt { get; set; } = DateTime.UtcNow;

    // 導航屬性
    /// <summary>
    /// 玩家角色
    /// </summary>
    public PlayerCharacter? PlayerCharacter { get; set; }

    /// <summary>
    /// 物品
    /// </summary>
    public Item? Item { get; set; }
}