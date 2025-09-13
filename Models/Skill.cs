using System.ComponentModel.DataAnnotations;

namespace MCPTRPGGame.Models;

/// <summary>
/// 技能模型
/// </summary>
public class Skill
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 技能名稱
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 技能描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 基礎成功率
    /// </summary>
    public int BaseSuccessRate { get; set; }

    /// <summary>
    /// 技能類別 (戰鬥、調查、交際等)
    /// </summary>
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// 是否為克蘇魯神話技能
    /// </summary>
    public bool IsCthulhuMythos { get; set; } = false;

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// 角色技能關聯模型
/// </summary>
public class CharacterSkill
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 角色 ID
    /// </summary>
    public int PlayerCharacterId { get; set; }

    /// <summary>
    /// 技能 ID
    /// </summary>
    public int SkillId { get; set; }

    /// <summary>
    /// 職業技能點數
    /// </summary>
    public int ProfessionalPoints { get; set; } = 0;

    /// <summary>
    /// 興趣技能點數
    /// </summary>
    public int HobbyPoints { get; set; } = 0;

    /// <summary>
    /// 經驗技能點數
    /// </summary>
    public int ExperiencePoints { get; set; } = 0;

    /// <summary>
    /// 總技能點數 (基礎 + 職業 + 興趣 + 經驗)
    /// </summary>
    public int TotalPoints => (Skill?.BaseSuccessRate ?? 0) + ProfessionalPoints + HobbyPoints + ExperiencePoints;

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 導航屬性
    /// <summary>
    /// 玩家角色
    /// </summary>
    public PlayerCharacter? PlayerCharacter { get; set; }

    /// <summary>
    /// 技能
    /// </summary>
    public Skill? Skill { get; set; }
}