using System.ComponentModel.DataAnnotations;

namespace MCPTRPGGame.Models;

/// <summary>
/// 角色模板 - 預設職業角色配置
/// </summary>
public class CharacterTemplate
{
    [Key]
    public int Id { get; set; }
    
    /// <summary>
    /// 職業名稱
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Occupation { get; set; } = string.Empty;
    
    /// <summary>
    /// 職業描述
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// 推薦年齡範圍
    /// </summary>
    [MaxLength(20)]
    public string AgeRange { get; set; } = string.Empty;
    
    /// <summary>
    /// 基礎屬性配置 (JSON格式)
    /// </summary>
    public string AttributeTemplate { get; set; } = string.Empty;
    
    /// <summary>
    /// 專業技能配置 (JSON格式)
    /// </summary>
    public string ProfessionalSkills { get; set; } = string.Empty;
    
    /// <summary>
    /// 背景故事模板
    /// </summary>
    public string BackgroundTemplate { get; set; } = string.Empty;
    
    /// <summary>
    /// 推薦特質
    /// </summary>
    public string RecommendedTraits { get; set; } = string.Empty;
    
    /// <summary>
    /// 職業特殊裝備
    /// </summary>
    public string SpecialEquipment { get; set; } = string.Empty;
    
    /// <summary>
    /// 是否為預設模板
    /// </summary>
    public bool IsDefault { get; set; } = true;
    
    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// 生成的角色配置
/// </summary>
public class GeneratedCharacterConfig
{
    public string Name { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Birthplace { get; set; } = string.Empty;
    public string BackgroundStory { get; set; } = string.Empty;
    public string ImportantPerson { get; set; } = string.Empty;
    public string Ideology { get; set; } = string.Empty;
    public string SignificantLocation { get; set; } = string.Empty;
    public string TreasuredPossession { get; set; } = string.Empty;
    public string Traits { get; set; } = string.Empty;
    
    // 屬性值
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Power { get; set; }
    public int Constitution { get; set; }
    public int Appearance { get; set; }
    public int Education { get; set; }
    public int Size { get; set; }
    public int Intelligence { get; set; }
    
    // 專業技能點數分配
    public Dictionary<string, int> ProfessionalSkillPoints { get; set; } = new();
}