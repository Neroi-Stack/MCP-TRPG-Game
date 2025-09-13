using System.ComponentModel.DataAnnotations;

namespace MCPTRPGGame.Models;

/// <summary>
/// 劇本模型
/// </summary>
public class Scenario
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 劇本名稱
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 劇本描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 劇本背景設定
    /// </summary>
    public string? Background { get; set; }

    /// <summary>
    /// 玩家開場白
    /// </summary>
    public string? OpeningNarrative { get; set; }

    /// <summary>
    /// 建議玩家人數
    /// </summary>
    public int RecommendedPlayerCount { get; set; } = 4;

    /// <summary>
    /// 預估遊戲時間 (小時)
    /// </summary>
    public int EstimatedDuration { get; set; } = 4;

    /// <summary>
    /// 難度等級 (1-10)
    /// </summary>
    public int DifficultyLevel { get; set; } = 5;

    /// <summary>
    /// 劇本標籤 (JSON格式)
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// 主要情節點 (JSON格式)
    /// </summary>
    public string? MainPlotPoints { get; set; }

    /// <summary>
    /// 真相和結局條件
    /// </summary>
    public string? TruthAndEndings { get; set; }

    /// <summary>
    /// KP 指導建議
    /// </summary>
    public string? KeeperNotes { get; set; }

    /// <summary>
    /// 預設檢定和 SAN 建議
    /// </summary>
    public string? DefaultRollsAndSanity { get; set; }

    /// <summary>
    /// 劇本狀態 (草稿、已發布、進行中、已完成)
    /// </summary>
    [MaxLength(20)]
    public string Status { get; set; } = "草稿";

    /// <summary>
    /// 創建者
    /// </summary>
    [MaxLength(100)]
    public string? CreatedBy { get; set; }

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
    /// 劇本場景
    /// </summary>
    public List<Scene> Scenes { get; set; } = new();

    /// <summary>
    /// 遊戲會話
    /// </summary>
    public List<GameSession> GameSessions { get; set; } = new();
}

/// <summary>
/// 遊戲會話模型
/// </summary>
public class GameSession
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 會話名稱
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 劇本 ID
    /// </summary>
    public int ScenarioId { get; set; }

    /// <summary>
    /// KP 名稱
    /// </summary>
    [MaxLength(100)]
    public string KeeperName { get; set; } = string.Empty;

    /// <summary>
    /// 會話狀態 (準備中、進行中、暫停、已結束)
    /// </summary>
    [MaxLength(20)]
    public string Status { get; set; } = "準備中";

    /// <summary>
    /// 當前場景 ID
    /// </summary>
    public int? CurrentSceneId { get; set; }

    /// <summary>
    /// 遊戲回合數
    /// </summary>
    public int CurrentRound { get; set; } = 0;

    /// <summary>
    /// 遊戲時間 (遊戲內時間描述)
    /// </summary>
    [MaxLength(100)]
    public string? GameTime { get; set; }

    /// <summary>
    /// 會話備註
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// 會話設定 (JSON格式，存儲各種遊戲設定)
    /// </summary>
    public string? Settings { get; set; }

    /// <summary>
    /// 開始時間
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// 結束時間
    /// </summary>
    public DateTime? EndedAt { get; set; }

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
    /// 劇本
    /// </summary>
    public Scenario? Scenario { get; set; }

    /// <summary>
    /// 會話參與的玩家角色
    /// </summary>
    public List<SessionCharacter> SessionCharacters { get; set; } = new();

    /// <summary>
    /// 遊戲記錄
    /// </summary>
    public List<GameLog> GameLogs { get; set; } = new();
}

/// <summary>
/// 會話角色關聯模型
/// </summary>
public class SessionCharacter
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 會話 ID
    /// </summary>
    public int GameSessionId { get; set; }

    /// <summary>
    /// 角色 ID
    /// </summary>
    public int PlayerCharacterId { get; set; }

    /// <summary>
    /// 是否為活躍角色
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// 加入時間
    /// </summary>
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 離開時間
    /// </summary>
    public DateTime? LeftAt { get; set; }

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