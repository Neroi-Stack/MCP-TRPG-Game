using System.ComponentModel.DataAnnotations;

namespace MCPTRPGGame.Models;

/// <summary>
/// 檢定記錄模型
/// </summary>
public class RollRecord
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 遊戲會話 ID
    /// </summary>
    public int? GameSessionId { get; set; }

    /// <summary>
    /// 角色 ID
    /// </summary>
    public int? PlayerCharacterId { get; set; }

    /// <summary>
    /// 檢定類型 (技能檢定、屬性檢定、對抗檢定等)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string RollType { get; set; } = string.Empty;

    /// <summary>
    /// 檢定目標 (技能名稱或屬性名稱)
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Target { get; set; } = string.Empty;

    /// <summary>
    /// 目標值
    /// </summary>
    public int TargetValue { get; set; }

    /// <summary>
    /// 骰子結果
    /// </summary>
    public int DiceResult { get; set; }

    /// <summary>
    /// 難度修正
    /// </summary>
    public int DifficultyModifier { get; set; } = 0;

    /// <summary>
    /// 檢定結果 (成功、失敗、大成功、大失敗)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Result { get; set; } = string.Empty;

    /// <summary>
    /// 成功等級 (一般成功、困難成功、極難成功)
    /// </summary>
    [MaxLength(20)]
    public string? SuccessLevel { get; set; }

    /// <summary>
    /// 檢定原因/情境
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// KP 備註
    /// </summary>
    public string? KeeperNotes { get; set; }

    /// <summary>
    /// 是否為秘密檢定
    /// </summary>
    public bool IsSecretRoll { get; set; } = false;

    /// <summary>
    /// 檢定時間
    /// </summary>
    public DateTime RolledAt { get; set; } = DateTime.UtcNow;

    // 導航屬性
    /// <summary>
    /// 遊戲會話
    /// </summary>
    public GameSession? GameSession { get; set; }

    /// <summary>
    /// 玩家角色
    /// </summary>
    public PlayerCharacter? PlayerCharacter { get; set; }
}

/// <summary>
/// SAN 值記錄模型
/// </summary>
public class SanityRecord
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 遊戲會話 ID
    /// </summary>
    public int? GameSessionId { get; set; }

    /// <summary>
    /// 角色 ID
    /// </summary>
    public int PlayerCharacterId { get; set; }

    /// <summary>
    /// SAN 值變化類型 (消耗、恢復、檢定失敗等)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string ChangeType { get; set; } = string.Empty;

    /// <summary>
    /// 變化數值 (正數為恢復，負數為消耗)
    /// </summary>
    public int ChangeValue { get; set; }

    /// <summary>
    /// 變化前 SAN 值
    /// </summary>
    public int PreviousSanity { get; set; }

    /// <summary>
    /// 變化後 SAN 值
    /// </summary>
    public int NewSanity { get; set; }

    /// <summary>
    /// 觸發原因
    /// </summary>
    [Required]
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// SAN 檢定結果 (如果適用)
    /// </summary>
    [MaxLength(20)]
    public string? CheckResult { get; set; }

    /// <summary>
    /// 骰子結果 (如果有檢定)
    /// </summary>
    public int? DiceResult { get; set; }

    /// <summary>
    /// 是否觸發不定性瘋狂
    /// </summary>
    public bool TriggeredTemporaryInsanity { get; set; } = false;

    /// <summary>
    /// 是否觸發無期限瘋狂
    /// </summary>
    public bool TriggeredIndefiniteInsanity { get; set; } = false;

    /// <summary>
    /// 瘋狂症狀描述
    /// </summary>
    public string? InsanitySymptoms { get; set; }

    /// <summary>
    /// KP 備註
    /// </summary>
    public string? KeeperNotes { get; set; }

    /// <summary>
    /// 記錄時間
    /// </summary>
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    // 導航屬性
    /// <summary>
    /// 遊戲會話
    /// </summary>
    public GameSession? GameSession { get; set; }

    /// <summary>
    /// 玩家角色
    /// </summary>
    public PlayerCharacter? PlayerCharacter { get; set; }
}

/// <summary>
/// 遊戲記錄模型
/// </summary>
public class GameLog
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 遊戲會話 ID
    /// </summary>
    public int GameSessionId { get; set; }

    /// <summary>
    /// 記錄類型 (敘述、行動、檢定、戰鬥、對話等)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string LogType { get; set; } = string.Empty;

    /// <summary>
    /// 記錄內容
    /// </summary>
    [Required]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 相關角色 ID
    /// </summary>
    public int? PlayerCharacterId { get; set; }

    /// <summary>
    /// 相關 NPC ID
    /// </summary>
    public int? NonPlayerCharacterId { get; set; }

    /// <summary>
    /// 相關場景 ID
    /// </summary>
    public int? SceneId { get; set; }

    /// <summary>
    /// 回合數
    /// </summary>
    public int? Round { get; set; }

    /// <summary>
    /// 遊戲內時間
    /// </summary>
    [MaxLength(100)]
    public string? GameTime { get; set; }

    /// <summary>
    /// 是否為重要事件
    /// </summary>
    public bool IsImportant { get; set; } = false;

    /// <summary>
    /// 是否為秘密記錄 (僅KP可見)
    /// </summary>
    public bool IsSecret { get; set; } = false;

    /// <summary>
    /// 標籤 (JSON格式)
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// 記錄時間
    /// </summary>
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    // 導航屬性
    /// <summary>
    /// 遊戲會話
    /// </summary>
    public GameSession? GameSession { get; set; }

    /// <summary>
    /// 玩家角色
    /// </summary>
    public PlayerCharacter? PlayerCharacter { get; set; }

    /// <summary>
    /// NPC
    /// </summary>
    public NonPlayerCharacter? NonPlayerCharacter { get; set; }

    /// <summary>
    /// 場景
    /// </summary>
    public Scene? Scene { get; set; }
}

/// <summary>
/// 角色記錄模型
/// </summary>
public class CharacterLog
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 角色 ID
    /// </summary>
    public int PlayerCharacterId { get; set; }

    /// <summary>
    /// 記錄類型 (屬性變化、物品獲得、技能提升等)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string LogType { get; set; } = string.Empty;

    /// <summary>
    /// 記錄內容
    /// </summary>
    [Required]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 數值變化 (如果適用)
    /// </summary>
    public int? ValueChange { get; set; }

    /// <summary>
    /// 變化前數值
    /// </summary>
    public int? PreviousValue { get; set; }

    /// <summary>
    /// 變化後數值
    /// </summary>
    public int? NewValue { get; set; }

    /// <summary>
    /// 記錄時間
    /// </summary>
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    // 導航屬性
    /// <summary>
    /// 玩家角色
    /// </summary>
    public PlayerCharacter? PlayerCharacter { get; set; }
}