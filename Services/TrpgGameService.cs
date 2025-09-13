using MCPTRPGGame.Data;
using MCPTRPGGame.Models;
using Microsoft.EntityFrameworkCore;

namespace MCPTRPGGame.Services;

/// <summary>
/// TRPG 遊戲邏輯服務
/// </summary>
public class TrpgGameService
{
    private readonly TrpgDbContext _context;
    private readonly Random _random = new();

    public TrpgGameService(TrpgDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 執行技能檢定
    /// </summary>
    /// <param name="characterId">角色ID</param>
    /// <param name="skillName">技能名稱</param>
    /// <param name="difficulty">難度修正 (0=普通, -20=困難, -40=極難)</param>
    /// <param name="sessionId">會話ID (可選)</param>
    /// <returns>檢定結果</returns>
    public async Task<RollResult> RollSkillCheckAsync(int characterId, string skillName, int difficulty = 0, int? sessionId = null)
    {
        var character = await _context.PlayerCharacters
            .Include(pc => pc.Skills)
            .ThenInclude(cs => cs.Skill)
            .FirstOrDefaultAsync(pc => pc.Id == characterId);

        if (character == null)
            throw new ArgumentException("角色不存在", nameof(characterId));

        var characterSkill = character.Skills.FirstOrDefault(cs =>
            cs.Skill?.Name.Equals(skillName, StringComparison.OrdinalIgnoreCase) == true);

        if (characterSkill == null)
            throw new ArgumentException($"角色沒有 {skillName} 技能", nameof(skillName));

        var targetValue = characterSkill.TotalPoints + difficulty;
        var diceResult = _random.Next(1, 101); // 1d100

        var result = DetermineRollResult(diceResult, targetValue);

        // 記錄檢定結果
        var rollRecord = new RollRecord
        {
            GameSessionId = sessionId,
            PlayerCharacterId = characterId,
            RollType = "技能檢定",
            Target = skillName,
            TargetValue = targetValue,
            DiceResult = diceResult,
            DifficultyModifier = difficulty,
            Result = result.Result,
            SuccessLevel = result.SuccessLevel,
            Reason = $"{character.Name} 進行 {skillName} 檢定"
        };

        _context.RollRecords.Add(rollRecord);
        await _context.SaveChangesAsync();

        return result;
    }

    /// <summary>
    /// 執行屬性檢定
    /// </summary>
    public async Task<RollResult> RollAttributeCheckAsync(int characterId, string attributeName, int difficulty = 0, int? sessionId = null)
    {
        var character = await _context.PlayerCharacters
            .FirstOrDefaultAsync(pc => pc.Id == characterId) ?? throw new ArgumentException("角色不存在", nameof(characterId));
        var attributeValue = GetAttributeValue(character, attributeName);
        var targetValue = attributeValue + difficulty;
        var diceResult = _random.Next(1, 101);

        var result = DetermineRollResult(diceResult, targetValue);

        // 記錄檢定結果
        var rollRecord = new RollRecord
        {
            GameSessionId = sessionId,
            PlayerCharacterId = characterId,
            RollType = "屬性檢定",
            Target = attributeName,
            TargetValue = targetValue,
            DiceResult = diceResult,
            DifficultyModifier = difficulty,
            Result = result.Result,
            SuccessLevel = result.SuccessLevel,
            Reason = $"{character.Name} 進行 {attributeName} 檢定"
        };

        _context.RollRecords.Add(rollRecord);
        await _context.SaveChangesAsync();

        return result;
    }

    /// <summary>
    /// 執行SAN值檢定
    /// </summary>
    public async Task<SanityCheckResult> RollSanityCheckAsync(int characterId, string sanityLoss, string reason, int? sessionId = null)
    {
        var character = await _context.PlayerCharacters
            .FirstOrDefaultAsync(pc => pc.Id == characterId) ?? throw new ArgumentException("角色不存在", nameof(characterId));
        var diceResult = _random.Next(1, 101);
        var isSuccess = diceResult <= character.CurrentSanity;

        // 解析SAN消耗 (格式: "失敗損失/成功損失", 例如: "1d10/1d4")
        var (failLoss, successLoss) = ParseSanityLoss(sanityLoss);
        var actualLoss = isSuccess ? successLoss : failLoss;

        var newSanity = Math.Max(0, character.CurrentSanity - actualLoss);
        var previousSanity = character.CurrentSanity;

        // 檢查是否觸發瘋狂
        var temporaryInsanity = false;
        var indefiniteInsanity = false;
        string? insanitySymptoms = null;

        if (actualLoss >= 5)
        {
            temporaryInsanity = true;
            insanitySymptoms = GenerateTemporaryInsanity();
        }

        if (newSanity <= character.Sanity / 5) // SAN值降到初始值的1/5以下
        {
            indefiniteInsanity = true;
            insanitySymptoms += " " + GenerateIndefiniteInsanity();
        }

        // 更新角色SAN值
        character.CurrentSanity = newSanity;
        _context.PlayerCharacters.Update(character);

        // 記錄SAN值變化
        var sanityRecord = new SanityRecord
        {
            GameSessionId = sessionId,
            PlayerCharacterId = characterId,
            ChangeType = "SAN檢定",
            ChangeValue = -actualLoss,
            PreviousSanity = previousSanity,
            NewSanity = newSanity,
            Reason = reason,
            CheckResult = isSuccess ? "成功" : "失敗",
            DiceResult = diceResult,
            TriggeredTemporaryInsanity = temporaryInsanity,
            TriggeredIndefiniteInsanity = indefiniteInsanity,
            InsanitySymptoms = insanitySymptoms
        };

        _context.SanityRecords.Add(sanityRecord);
        await _context.SaveChangesAsync();

        return new SanityCheckResult
        {
            IsSuccess = isSuccess,
            DiceResult = diceResult,
            SanityLoss = actualLoss,
            NewSanityValue = newSanity,
            TriggeredTemporaryInsanity = temporaryInsanity,
            TriggeredIndefiniteInsanity = indefiniteInsanity,
            InsanitySymptoms = insanitySymptoms
        };
    }

    private RollResult DetermineRollResult(int diceResult, int targetValue)
    {
        var result = new RollResult
        {
            DiceResult = diceResult,
            TargetValue = targetValue
        };

        if (diceResult == 100 || (diceResult >= 96 && targetValue < 50))
        {
            result.Result = "大失敗";
            result.SuccessLevel = null;
            return result;
        }

        if (diceResult == 1)
        {
            result.Result = "大成功";
            result.SuccessLevel = "大成功";
            return result;
        }

        if (diceResult <= targetValue)
        {
            result.Result = "成功";
            if (diceResult <= targetValue / 5)
                result.SuccessLevel = "極難成功";
            else if (diceResult <= targetValue / 2)
                result.SuccessLevel = "困難成功";
            else
                result.SuccessLevel = "一般成功";

            return result;
        }

        result.Result = "失敗";
        result.SuccessLevel = null;
        return result;
    }

    private int GetAttributeValue(PlayerCharacter character, string attributeName)
    {
        return attributeName.ToLower() switch
        {
            "str" or "力量" => character.Strength,
            "dex" or "敏捷" => character.Dexterity,
            "pow" or "意志" => character.Power,
            "con" or "體質" => character.Constitution,
            "app" or "外貌" => character.Appearance,
            "edu" or "教育" => character.Education,
            "siz" or "體型" => character.Size,
            "int" or "智力" => character.Intelligence,
            "san" or "理智" => character.CurrentSanity,
            "luck" or "幸運" => character.CurrentLuck,
            _ => throw new ArgumentException($"未知的屬性: {attributeName}")
        };
    }

    private (int failLoss, int successLoss) ParseSanityLoss(string sanityLoss)
    {
        var parts = sanityLoss.Split('/');
        if (parts.Length != 2)
            throw new ArgumentException("SAN消耗格式錯誤，應為 '失敗損失/成功損失'");

        var failLoss = RollDice(parts[0].Trim());
        var successLoss = RollDice(parts[1].Trim());

        return (failLoss, successLoss);
    }

    private int RollDice(string diceExpression)
    {
        // 簡單的骰子解析，支援 "XdY" 和 "XdY+Z" 格式
        diceExpression = diceExpression.ToLower().Replace(" ", "");

        if (int.TryParse(diceExpression, out var fixedValue))
            return fixedValue;

        var parts = diceExpression.Split('d');
        if (parts.Length != 2)
            throw new ArgumentException($"無效的骰子表達式: {diceExpression}");

        var diceCount = int.Parse(parts[0]);
        var diceSides = parts[1];

        var bonus = 0;
        if (diceSides.Contains('+'))
        {
            var bonusParts = diceSides.Split('+');
            diceSides = bonusParts[0];
            bonus = int.Parse(bonusParts[1]);
        }

        var sides = int.Parse(diceSides);
        var total = 0;

        for (int i = 0; i < diceCount; i++)
        {
            total += _random.Next(1, sides + 1);
        }

        return total + bonus;
    }

    private string GenerateTemporaryInsanity()
    {
        var symptoms = new[]
        {
            "昏厥1d10輪",
            "歇斯底里大笑或哭泣1d10輪",
            "逃跑1d10輪",
            "呆立不動1d10輪",
            "重複動作1d10輪",
            "攻擊最近的人1d10輪"
        };

        return symptoms[_random.Next(symptoms.Length)];
    }

    private string GenerateIndefiniteInsanity()
    {
        var symptoms = new[]
        {
            "妄想症",
            "偏執狂",
            "恐懼症",
            "狂躁症",
            "憂鬱症",
            "強迫症",
            "解離症"
        };

        return symptoms[_random.Next(symptoms.Length)];
    }
}

/// <summary>
/// 檢定結果
/// </summary>
public class RollResult
{
    public string Result { get; set; } = string.Empty;
    public string? SuccessLevel { get; set; }
    public int DiceResult { get; set; }
    public int TargetValue { get; set; }
}

/// <summary>
/// SAN值檢定結果
/// </summary>
public class SanityCheckResult
{
    public bool IsSuccess { get; set; }
    public int DiceResult { get; set; }
    public int SanityLoss { get; set; }
    public int NewSanityValue { get; set; }
    public bool TriggeredTemporaryInsanity { get; set; }
    public bool TriggeredIndefiniteInsanity { get; set; }
    public string? InsanitySymptoms { get; set; }
}