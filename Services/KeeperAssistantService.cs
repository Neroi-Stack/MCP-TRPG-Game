using MCPTRPGGame.Data;
using MCPTRPGGame.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MCPTRPGGame.Services;

/// <summary>
/// KP 輔助功能服務
/// </summary>
public class KeeperAssistantService
{
    private readonly TrpgDbContext _context;
    private readonly Random _random = new();

    public KeeperAssistantService(TrpgDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 生成場景描述
    /// </summary>
    public async Task<string> GenerateSceneDescriptionAsync(int sceneId, bool includeHiddenElements = false)
    {
        var scene = await _context.Scenes
            .Include(s => s.NPCs)
            .Include(s => s.Items)
            .ThenInclude(si => si.Item)
            .FirstOrDefaultAsync(s => s.Id == sceneId);

        if (scene == null)
            return "場景不存在";

        var description = $"**{scene.Name}**\n\n";
        description += $"{scene.Description}\n\n";

        // 詳細描述
        if (!string.IsNullOrEmpty(scene.DetailedDescription))
        {
            description += $"🔍 **詳細觀察**:\n{scene.DetailedDescription}\n\n";
        }

        // 環境資訊
        description += "🌍 **環境資訊**:\n";
        description += $"• 光線條件: {scene.LightingCondition}\n";
        description += $"• 溫度: {scene.Temperature}\n";
        
        if (!string.IsNullOrEmpty(scene.SoundEnvironment))
            description += $"• 聲音環境: {scene.SoundEnvironment}\n";
        
        if (!string.IsNullOrEmpty(scene.Smell))
            description += $"• 氣味: {scene.Smell}\n";

        description += $"• 氛圍: {scene.Atmosphere}\n\n";

        // 可見的NPC
        var visibleNpcs = scene.NPCs.Where(npc => npc.Status != "隱藏").ToList();
        if (visibleNpcs.Any())
        {
            description += "👥 **場景中的人物**:\n";
            foreach (var npc in visibleNpcs)
            {
                description += $"• **{npc.Name}**: {npc.Appearance}\n";
            }
            description += "\n";
        }

        // 可見的物品
        var visibleItems = scene.Items.Where(si => si.IsDiscovered && !si.IsHidden).ToList();
        if (visibleItems.Any())
        {
            description += "📦 **可見物品**:\n";
            foreach (var item in visibleItems)
            {
                description += $"• {item.Item?.Name}";
                if (item.Quantity > 1) description += $" x{item.Quantity}";
                description += "\n";
            }
            description += "\n";
        }

        // KP專用隱藏資訊
        if (includeHiddenElements)
        {
            description += "🔒 **KP專用資訊**:\n";
            
            if (!string.IsNullOrEmpty(scene.HiddenClues))
                description += $"💡 隱藏線索: {scene.HiddenClues}\n";

            if (!string.IsNullOrEmpty(scene.EventTriggers))
                description += $"⚡ 事件觸發: {scene.EventTriggers}\n";

            if (!string.IsNullOrEmpty(scene.SanityCheckTrigger))
                description += $"🧠 SAN檢定觸發: {scene.SanityCheckTrigger} (消耗: {scene.SanityLoss})\n";

            // 隱藏的物品
            var hiddenItems = scene.Items.Where(si => si.IsHidden && !si.IsDiscovered).ToList();
            if (hiddenItems.Any())
            {
                description += "🔍 **隱藏物品**:\n";
                foreach (var item in hiddenItems)
                {
                    description += $"• {item.Item?.Name}";
                    if (!string.IsNullOrEmpty(item.RequiredSkillToFind))
                        description += $" (需要 {item.RequiredSkillToFind} 檢定)";
                    description += "\n";
                }
            }

            // 隱藏的NPC
            var hiddenNpcs = scene.NPCs.Where(npc => npc.Status == "隱藏").ToList();
            if (hiddenNpcs.Any())
            {
                description += "👻 **隱藏NPC**: ";
                description += string.Join(", ", hiddenNpcs.Select(npc => npc.Name)) + "\n";
            }
        }

        return description;
    }

    /// <summary>
    /// 生成NPC對話選項
    /// </summary>
    public async Task<string> GenerateNpcDialogueAsync(int npcId, string topic)
    {
        var npc = await _context.NonPlayerCharacters.FindAsync(npcId);
        if (npc == null)
            return "NPC不存在";

        var response = $"💬 **{npc.Name}** 對於 \"{topic}\" 的回應:\n\n";

        // 解析對話選項
        if (!string.IsNullOrEmpty(npc.DialogueOptions))
        {
            try
            {
                var dialogueOptions = JsonSerializer.Deserialize<DialogueOption[]>(npc.DialogueOptions);
                var relevantOption = dialogueOptions?.FirstOrDefault(opt => 
                    opt.Topic.Contains(topic, StringComparison.OrdinalIgnoreCase) ||
                    topic.Contains(opt.Topic, StringComparison.OrdinalIgnoreCase));

                if (relevantOption != null)
                {
                    response += $"\"{relevantOption.Response}\"";
                }
                else
                {
                    response += GenerateGenericResponse(npc, topic);
                }
            }
            catch
            {
                response += GenerateGenericResponse(npc, topic);
            }
        }
        else
        {
            response += GenerateGenericResponse(npc, topic);
        }

        // 添加NPC性格提示
        response += $"\n\n🎭 **性格提示**: {npc.Personality}";

        return response;
    }

    /// <summary>
    /// 建議檢定和難度
    /// </summary>
    public string SuggestRollsForScene(int sceneId, string playerAction)
    {
        var suggestions = new List<string>();

        // 根據場景和行動建議檢定
        var actionLower = playerAction.ToLower();

        if (actionLower.Contains("尋找") || actionLower.Contains("搜索") || actionLower.Contains("調查"))
        {
            suggestions.Add("偵查 (發現明顯線索)");
            suggestions.Add("圖書館使用 (文字資料)");
            suggestions.Add("聆聽 (聲音線索)");
        }

        if (actionLower.Contains("交談") || actionLower.Contains("詢問") || actionLower.Contains("說服"))
        {
            suggestions.Add("魅惑 (友善交談)");
            suggestions.Add("說服 (改變想法)");
            suggestions.Add("快速交談 (套取資訊)");
            suggestions.Add("恐嚇 (威脅逼供)");
        }

        if (actionLower.Contains("攀爬") || actionLower.Contains("跳躍") || actionLower.Contains("體能"))
        {
            suggestions.Add("攀爬");
            suggestions.Add("跳躍");
            suggestions.Add("力量 (屬性檢定)");
            suggestions.Add("敏捷 (屬性檢定)");
        }

        if (actionLower.Contains("開鎖") || actionLower.Contains("修理") || actionLower.Contains("技術"))
        {
            suggestions.Add("開鎖");
            suggestions.Add("機械維修");
            suggestions.Add("電器維修");
        }

        if (actionLower.Contains("醫療") || actionLower.Contains("治療") || actionLower.Contains("檢查身體"))
        {
            suggestions.Add("急救");
            suggestions.Add("醫學");
        }

        if (actionLower.Contains("神秘") || actionLower.Contains("符號") || actionLower.Contains("儀式"))
        {
            suggestions.Add("神秘學");
            suggestions.Add("克蘇魯神話 (危險!)");
            suggestions.Add("歷史");
        }

        var result = $"🎲 **建議的檢定** (針對行動: {playerAction})\n\n";

        if (suggestions.Any())
        {
            result += "**推薦檢定**:\n";
            foreach (var suggestion in suggestions)
            {
                result += $"• {suggestion}\n";
            }
        }
        else
        {
            result += "**通用檢定**:\n";
            result += "• 偵查 (一般觀察)\n";
            result += "• 聆聽 (聲音)\n";
            result += "• 意志 (屬性檢定)\n";
            result += "• 幸運 (運氣成分)\n";
        }

        result += "\n**難度建議**:\n";
        result += "• 簡單任務: +20 修正\n";
        result += "• 普通任務: 無修正\n";
        result += "• 困難任務: -20 修正\n";
        result += "• 極難任務: -40 修正\n";

        return result;
    }

    /// <summary>
    /// 生成隨機事件
    /// </summary>
    public string GenerateRandomEvent(string sceneType, int dangerLevel)
    {
        var events = sceneType.ToLower() switch
        {
            "室內" => new[]
            {
                "聽到樓上傳來腳步聲",
                "門突然無風自動關閉",
                "燈光閃爍不定",
                "牆壁發出奇怪的響聲",
                "感覺到有人在背後注視",
                "房間溫度突然下降"
            },
            "室外" => new[]
            {
                "烏雲遮住月光",
                "遠方傳來野獸嚎叫",
                "風聲中似乎有人在呼喊",
                "看到遠處有人影晃動",
                "地面出現奇怪的腳印",
                "突然起霧，視線變得模糊"
            },
            "地下室" => new[]
            {
                "水滴聲越來越急促",
                "空氣中瀰漫著霉味",
                "牆壁上出現新的裂縫",
                "聽到遠處傳來低語聲",
                "感覺到地面在輕微震動",
                "看到牆上的影子在移動"
            },
            _ => new[]
            {
                "感到一陣莫名的寒意",
                "聽到奇怪的聲響",
                "環境氣氛變得詭異",
                "感覺有什麼不對勁",
                "空氣中瀰漫著不祥的預感"
            }
        };

        var selectedEvent = events[_random.Next(events.Length)];
        var intensity = dangerLevel > 5 ? "強烈" : dangerLevel > 3 ? "明顯" : "輕微";

        return $"🎭 **隨機事件** ({intensity})\n\n" +
               $"{selectedEvent}\n\n" +
               $"💡 **KP提示**: 可要求相關的 SAN 檢定或技能檢定";
    }

    /// <summary>
    /// 戰鬥輔助
    /// </summary>
    public string GenerateCombatAssistance(string weaponType, int characterDamageBonus = 0)
    {
        var weaponStats = weaponType.ToLower() switch
        {
            "拳頭" or "格鬥" => new { Damage = "1d3", Skill = "格鬥", Range = "接觸" },
            "小刀" or "刀" => new { Damage = "1d4", Skill = "格鬥", Range = "接觸" },
            "手槍" or "左輪" => new { Damage = "1d10", Skill = "手槍", Range = "15m" },
            "步槍" => new { Damage = "2d6", Skill = "步槍", Range = "90m" },
            "霰彈槍" => new { Damage = "4d6/2d6/1d6", Skill = "霰彈槍", Range = "10m/20m/50m" },
            _ => new { Damage = "1d6", Skill = "格鬥", Range = "接觸" }
        };

        var result = $"⚔️ **戰鬥輔助 - {weaponType}**\n\n";
        result += $"🎯 **基礎傷害**: {weaponStats.Damage}\n";
        result += $"🎲 **使用技能**: {weaponStats.Skill}\n";
        result += $"📏 **有效射程**: {weaponStats.Range}\n";

        if (characterDamageBonus != 0)
        {
            result += $"💪 **傷害加成**: {characterDamageBonus:+#;-#;0}\n";
        }

        result += "\n📋 **戰鬥流程提醒**:\n";
        result += "1. 宣告行動\n";
        result += "2. 決定先制權 (敏捷對抗)\n";
        result += "3. 進行攻擊檢定\n";
        result += "4. 計算傷害\n";
        result += "5. 檢查是否昏迷/死亡\n";

        return result;
    }

    private string GenerateGenericResponse(NonPlayerCharacter npc, string topic)
    {
        var responses = npc.Type.ToLower() switch
        {
            "友好" => new[]
            {
                $"關於{topic}嗎？我想想看...",
                $"啊，{topic}，這讓我想起了一些事情。",
                $"我很樂意談談{topic}。"
            },
            "中立" => new[]
            {
                $"關於{topic}...我不確定我知道多少。",
                $"這個話題...{topic}...有什麼特別的嗎？",
                $"我對{topic}了解有限。"
            },
            "敵對" => new[]
            {
                $"我為什麼要告訴你關於{topic}的事？",
                $"{topic}？這不關你的事！",
                $"別指望我會談論{topic}。"
            },
            _ => new[]
            {
                $"關於{topic}...",
                $"你問的是{topic}嗎？",
                $"我不太確定該說些什麼關於{topic}。"
            }
        };

        return responses[_random.Next(responses.Length)];
    }

    /// <summary>
    /// 對話選項類別
    /// </summary>
    private class DialogueOption
    {
        public string Topic { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
    }
}