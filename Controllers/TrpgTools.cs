using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using System.ComponentModel;
using MCPTRPGGame.Data;
using MCPTRPGGame.Services;
using MCPTRPGGame.Models;

namespace MCPTRPGGame.Controllers;


[McpServerToolType]
public static class TrpgTools
{
    private static IServiceProvider? _serviceProvider;

    public static void Initialize(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [McpServerTool, Description("🎮 開始TRPG冒險！這是您遊玩TRPG的入口工具。會幫您創建角色並開始《沉睡之館》劇本。")]
    public static string StartTrpgAdventure()
    {
        if (_serviceProvider == null) return "服務未初始化";

        try
        {
            return "您是Keeper，請先使用 `CreateSleepingManorScenario` 工具來創建《沉睡之館》劇本，然後使用 `CreateGameSession` 工具來創建遊戲會話。接著，使用 `GetAvailableCharacterTemplates` 查看可用的角色職業模板，並使用 `GenerateRandomCharacter` 生成隨機角色供玩家選擇。最後，使用 `GetCharacter` 查看您的角色資訊，準備開始冒險吧！";
        }
        catch (Exception ex)
        {
            return $"❌ 開始冒險失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("創建新的玩家角色")]
    public static async Task<string> CreateCharacter(string name, string playerName, string occupation = "", int age = 25)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var characterService = scope.ServiceProvider.GetRequiredService<CharacterService>();

        try
        {
            var character = await characterService.CreateCharacterAsync(name, playerName, occupation, age);
            return $"✅ 角色創建成功！\n" +
                   $"🎭 角色名稱: {character.Name}\n" +
                   $"👤 玩家: {character.PlayerName}\n" +
                   $"💼 職業: {character.Occupation}\n" +
                   $"🎂 年齡: {character.Age}\n" +
                   $"🆔 角色ID: {character.Id}";
        }
        catch (Exception ex)
        {
            return $"❌ 角色創建失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("獲取角色資訊")]
    public static async Task<string> GetCharacter(int characterId)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var characterService = scope.ServiceProvider.GetRequiredService<CharacterService>();

        try
        {
            var character = await characterService.GetCharacterAsync(characterId);
            if (character == null)
                return "❌ 角色不存在";

            var result = $"🎭 **{character.Name}** (玩家: {character.PlayerName})\n" +
                        $"💼 職業: {character.Occupation} | 🎂 年齡: {character.Age} | ⚖️ 狀態: {character.Status}\n\n" +
                        $"📊 **屬性值**\n" +
                        $"💪 力量: {character.Strength} | 🏃 敏捷: {character.Dexterity} | 🧠 智力: {character.Intelligence}\n" +
                        $"💫 意志: {character.Power} | 🛡️ 體質: {character.Constitution} | 📏 體型: {character.Size}\n" +
                        $"🌟 外貌: {character.Appearance} | 🎓 教育: {character.Education}\n\n" +
                        $"❤️ 生命值: {character.CurrentHitPoints}/{character.HitPoints}\n" +
                        $"🔮 魔法值: {character.CurrentMagicPoints}/{character.MagicPoints}\n" +
                        $"🧠 理智值: {character.CurrentSanity}/{character.Sanity}\n" +
                        $"🍀 幸運值: {character.CurrentLuck}/{character.Luck}\n";

            if (character.Skills.Any())
            {
                result += "\n🎯 **技能** (前10項)\n";
                var topSkills = character.Skills.Take(10);
                foreach (var skill in topSkills)
                {
                    result += $"• {skill.Skill?.Name}: {skill.TotalPoints}%\n";
                }
            }

            if (character.Items.Any())
            {
                result += "\n🎒 **物品**\n";
                foreach (var item in character.Items.Take(10))
                {
                    result += $"• {item.Item?.Name} x{item.Quantity}";
                    if (item.IsEquipped) result += " (已裝備)";
                    result += "\n";
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            return $"❌ 獲取角色資訊失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("進行技能檢定")]
    public static async Task<string> RollSkillCheck(int characterId, string skillName, int difficulty = 0, int? sessionId = null)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var gameService = scope.ServiceProvider.GetRequiredService<TrpgGameService>();
        var characterService = scope.ServiceProvider.GetRequiredService<CharacterService>();

        try
        {
            var character = await characterService.GetCharacterAsync(characterId);
            if (character == null)
                return "❌ 角色不存在";

            var result = await gameService.RollSkillCheckAsync(characterId, skillName, difficulty, sessionId);

            var difficultyText = difficulty switch
            {
                -40 => " (極難)",
                -20 => " (困難)",
                0 => "",
                _ => $" ({difficulty:+#;-#})"
            };

            return $"🎲 **{character.Name}** 進行 **{skillName}** 檢定{difficultyText}\n" +
                   $"📊 目標值: {result.TargetValue}% | 骰子: {result.DiceResult}\n" +
                   $"🎯 結果: **{result.Result}**" +
                   (result.SuccessLevel != null ? $" ({result.SuccessLevel})" : "") + "\n";
        }
        catch (Exception ex)
        {
            return $"❌ 檢定失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("進行屬性檢定")]
    public static async Task<string> RollAttributeCheck(int characterId, string attributeName, int difficulty = 0, int? sessionId = null)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var gameService = scope.ServiceProvider.GetRequiredService<TrpgGameService>();
        var characterService = scope.ServiceProvider.GetRequiredService<CharacterService>();

        try
        {
            var character = await characterService.GetCharacterAsync(characterId);
            if (character == null)
                return "❌ 角色不存在";

            var result = await gameService.RollAttributeCheckAsync(characterId, attributeName, difficulty, sessionId);

            var difficultyText = difficulty switch
            {
                -40 => " (極難)",
                -20 => " (困難)",
                0 => "",
                _ => $" ({difficulty:+#;-#})"
            };

            return $"🎲 **{character.Name}** 進行 **{attributeName}** 檢定{difficultyText}\n" +
                   $"🎯 結果: **{result.Result}**" +
                   (result.SuccessLevel != null ? $" ({result.SuccessLevel})" : "") + "\n";
        }
        catch (Exception ex)
        {
            return $"❌ 檢定失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("進行SAN值檢定")]
    public static async Task<string> RollSanityCheck(int characterId, string sanityLoss, string reason, int? sessionId = null)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var gameService = scope.ServiceProvider.GetRequiredService<TrpgGameService>();
        var characterService = scope.ServiceProvider.GetRequiredService<CharacterService>();

        try
        {
            var character = await characterService.GetCharacterAsync(characterId);
            if (character == null)
                return "❌ 角色不存在";

            var result = await gameService.RollSanityCheckAsync(characterId, sanityLoss, reason, sessionId);

            var output = $"🧠 **{character.Name}** 進行 SAN 檢定\n" +
                        $"💭 原因: {reason}\n" +
                        $"🎯 結果: **{(result.IsSuccess ? "成功" : "失敗")}**\n" +
                        $"💔 SAN 消耗: {result.SanityLoss}\n" +
                        $"🧠 當前 SAN: {result.NewSanityValue}\n";

            if (result.TriggeredTemporaryInsanity || result.TriggeredIndefiniteInsanity)
            {
                output += "⚠️ **觸發瘋狂症狀**\n";
                if (result.InsanitySymptoms != null)
                    output += $"🎭 症狀: {result.InsanitySymptoms}\n";
            }

            return output;
        }
        catch (Exception ex)
        {
            return $"❌ SAN檢定失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("更新角色生命值")]
    public static async Task<string> UpdateCharacterHitPoints(int characterId, int newHitPoints)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var characterService = scope.ServiceProvider.GetRequiredService<CharacterService>();

        try
        {
            await characterService.UpdateCharacterHitPointsAsync(characterId, newHitPoints);
            var character = await characterService.GetCharacterAsync(characterId);

            return $"❤️ **{character?.Name}** 的生命值已更新為 {character?.CurrentHitPoints}/{character?.HitPoints}";
        }
        catch (Exception ex)
        {
            return $"❌ 更新生命值失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("獲取所有角色列表")]
    public static async Task<string> GetAllCharacters()
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var characterService = scope.ServiceProvider.GetRequiredService<CharacterService>();

        try
        {
            var characters = await characterService.GetAllCharactersAsync();
            if (!characters.Any())
                return "📝 目前沒有任何角色";

            var result = "👥 **角色列表**\n\n";
            foreach (var character in characters)
            {
                result += $"🎭 **{character.Name}** (ID: {character.Id})\n" +
                         $"👤 玩家: {character.PlayerName} | 💼 職業: {character.Occupation}\n" +
                         $"❤️ HP: {character.CurrentHitPoints}/{character.HitPoints} | " +
                         $"🧠 SAN: {character.CurrentSanity}/{character.Sanity} | " +
                         $"⚖️ 狀態: {character.Status}\n\n";
            }

            return result;
        }
        catch (Exception ex)
        {
            return $"❌ 獲取角色列表失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("創建《沉睡之館》劇本")]
    public static async Task<string> CreateSleepingManorScenario()
    {
        if (_serviceProvider == null) return "service not initialized";

        using var scope = _serviceProvider.CreateScope();
        var scenarioService = scope.ServiceProvider.GetRequiredService<ScenarioService>();
        var context = scope.ServiceProvider.GetRequiredService<TrpgDbContext>();
        try
        {
            var scenario = await context.Scenarios.FirstOrDefaultAsync(s => s.Name == "沉眠之館");
            return $"✅ **《{scenario?.Name}》劇本創建成功！**\n" +
                   $"📖 劇本名稱: {scenario?.Name}\n" +
                   $"🆔 劇本ID: {scenario?.Id}\n" +
                   $"👥 建議玩家數: {scenario?.RecommendedPlayerCount}\n" +
                   $"⏱️ 預估時間: {scenario?.EstimatedDuration} 小時\n" +
                   $"⭐ 難度等級: {scenario?.DifficultyLevel}/10\n\n" +
                   $"📝 背景設定已載入，包含所有場景和NPC資料。";
        }
        catch (Exception ex)
        {
            return $"❌ 創建劇本失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("創建新的遊戲會話")]
    public static async Task<string> CreateGameSession(int scenarioId, string sessionName, string keeperName)
    {
        if (_serviceProvider == null) return "service not initialized";

        using var scope = _serviceProvider.CreateScope();
        var scenarioService = scope.ServiceProvider.GetRequiredService<ScenarioService>();

        try
        {
            var session = await scenarioService.CreateGameSessionAsync(scenarioId, sessionName, keeperName);
            return $"✅ **遊戲會話創建成功！**\n" +
                   $"🎮 會話名稱: {session.Name}\n" +
                   $"🎭 KP: {session.KeeperName}\n" +
                   $"🆔 會話ID: {session.Id}\n" +
                   $"⚖️ 狀態: {session.Status}\n" +
                   $"🕐 遊戲時間: {session.GameTime}";
        }
        catch (Exception ex)
        {
            return $"❌ 創建遊戲會話失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("獲取劇本資訊")]
    public static async Task<string> GetScenarioInfo(int scenarioId)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var scenarioService = scope.ServiceProvider.GetRequiredService<ScenarioService>();

        try
        {
            var scenario = await scenarioService.GetScenarioAsync(scenarioId);
            if (scenario == null)
                return "❌ 劇本不存在";

            var result = $"📖 **{scenario.Name}**\n\n" +
                        $"📝 **描述**: {scenario.Description}\n\n" +
                        $"🎬 **開場白**:\n{scenario.OpeningNarrative}\n\n" +
                        $"ℹ️ **基本資訊**:\n" +
                        $"• 建議玩家數: {scenario.RecommendedPlayerCount}\n" +
                        $"• 預估時間: {scenario.EstimatedDuration} 小時\n" +
                        $"• 難度等級: {scenario.DifficultyLevel}/10\n" +
                        $"• 狀態: {scenario.Status}\n\n";

            if (scenario.Scenes.Any())
            {
                result += $"🗺️ **場景列表** ({scenario.Scenes.Count}個):\n";
                foreach (var scene in scenario.Scenes.Take(5))
                {
                    result += $"• {scene.Name} ({scene.Type})\n";
                }
                if (scenario.Scenes.Count > 5)
                    result += $"• ... 及其他 {scenario.Scenes.Count - 5} 個場景\n";
            }

            return result;
        }
        catch (Exception ex)
        {
            return $"❌ 獲取劇本資訊失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("獲取場景詳細資訊")]
    public static async Task<string> GetSceneInfo(string sceneName)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TrpgDbContext>();

        try
        {
            var scene = await context.Scenes
                .Include(s => s.NPCs)
                .Include(s => s.Items)
                .ThenInclude(si => si.Item)
                .FirstOrDefaultAsync(s => s.Name.Contains(sceneName));

            if (scene == null)
                return $"❌ 找不到名稱包含 '{sceneName}' 的場景";

            var result = $"🗺️ **{scene.Name}**\n\n" +
                        $"📝 **描述**: {scene.Description}\n\n" +
                        $"🔍 **詳細描述**: {scene.DetailedDescription}\n\n" +
                        $"ℹ️ **場景資訊**:\n" +
                        $"• 類型: {scene.Type}\n" +
                        $"• 氛圍: {scene.Atmosphere}\n" +
                        $"• 光線: {scene.LightingCondition}\n" +
                        $"• 危險等級: {scene.DangerLevel}/10\n";

            if (!string.IsNullOrEmpty(scene.HiddenClues))
            {
                result += $"\n🔍 **隱藏線索**: {scene.HiddenClues}\n";
            }

            if (!string.IsNullOrEmpty(scene.SoundEnvironment))
            {
                result += $"\n🔊 **聲音環境**: {scene.SoundEnvironment}\n";
            }

            if (!string.IsNullOrEmpty(scene.SanityLoss))
            {
                result += $"\n🧠 **SAN消耗**: {scene.SanityLoss}\n";
            }

            if (scene.NPCs.Any())
            {
                result += $"\n👥 **場景中的NPC**:\n";
                foreach (var npc in scene.NPCs)
                {
                    result += $"• {npc.Name} ({npc.Type})\n";
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            return $"❌ 獲取場景資訊失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("獲取NPC資訊")]
    public static async Task<string> GetNpcInfo(string npcName)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TrpgDbContext>();

        try
        {
            var npc = await context.NonPlayerCharacters
                .Include(n => n.Scene)
                .FirstOrDefaultAsync(n => n.Name.Contains(npcName));

            if (npc == null)
                return $"❌ 找不到名稱包含 '{npcName}' 的NPC";

            var result = $"👤 **{npc.Name}**\n\n" +
                        $"📝 **描述**: {npc.Description}\n\n" +
                        $"👁️ **外觀**: {npc.Appearance}\n\n" +
                        $"🎭 **性格**: {npc.Personality}\n\n" +
                        $"🎯 **動機**: {npc.Motivation}\n\n" +
                        $"📚 **背景**: {npc.Background}\n\n" +
                        $"ℹ️ **基本資訊**:\n" +
                        $"• 類型: {npc.Type}\n" +
                        $"• 狀態: {npc.Status}\n";

            if (npc.Scene != null)
            {
                result += $"• 位置: {npc.Scene.Name}\n";
            }

            if (npc.HitPoints.HasValue)
            {
                result += $"• 生命值: {npc.CurrentHitPoints}/{npc.HitPoints}\n";
            }

            if (!string.IsNullOrEmpty(npc.SanityLoss))
            {
                result += $"• SAN消耗: {npc.SanityLoss}\n";
            }

            if (!string.IsNullOrEmpty(npc.AvailableInformation))
            {
                result += $"\n💡 **可提供情報**: {npc.AvailableInformation}\n";
            }

            if (!string.IsNullOrEmpty(npc.SpecialAbilities))
            {
                result += $"\n✨ **特殊能力**: {npc.SpecialAbilities}\n";
            }

            return result;
        }
        catch (Exception ex)
        {
            return $"❌ 獲取NPC資訊失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("記錄遊戲事件")]
    public static async Task<string> LogGameEvent(int sessionId, string logType, string content, int? characterId = null, int? npcId = null, int? sceneId = null)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TrpgDbContext>();

        try
        {
            var gameLog = new GameLog
            {
                GameSessionId = sessionId,
                LogType = logType,
                Content = content,
                PlayerCharacterId = characterId,
                NonPlayerCharacterId = npcId,
                SceneId = sceneId
            };

            context.GameLogs.Add(gameLog);
            await context.SaveChangesAsync();

            return $"📝 遊戲事件已記錄: {content}";
        }
        catch (Exception ex)
        {
            return $"❌ 記錄事件失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("生成場景描述 (KP輔助)")]
    public static async Task<string> GenerateSceneDescription(int sceneId, bool includeHiddenElements = false)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var keeperService = scope.ServiceProvider.GetRequiredService<KeeperAssistantService>();

        try
        {
            return await keeperService.GenerateSceneDescriptionAsync(sceneId, includeHiddenElements);
        }
        catch (Exception ex)
        {
            return $"❌ 生成場景描述失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("生成NPC對話 (KP輔助)")]
    public static async Task<string> GenerateNpcDialogue(int npcId, string topic)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var keeperService = scope.ServiceProvider.GetRequiredService<KeeperAssistantService>();

        try
        {
            return await keeperService.GenerateNpcDialogueAsync(npcId, topic);
        }
        catch (Exception ex)
        {
            return $"❌ 生成NPC對話失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("建議檢定和難度 (KP輔助)")]
    public static string SuggestRolls(int sceneId, string playerAction)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var keeperService = scope.ServiceProvider.GetRequiredService<KeeperAssistantService>();

        try
        {
            return keeperService.SuggestRollsForScene(sceneId, playerAction);
        }
        catch (Exception ex)
        {
            return $"❌ 建議檢定失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("生成隨機事件 (KP輔助)")]
    public static Task<string> GenerateRandomEvent(string sceneType, int dangerLevel = 3)
    {
        if (_serviceProvider == null) return Task.FromResult("服務未初始化");

        using var scope = _serviceProvider.CreateScope();
        var keeperService = scope.ServiceProvider.GetRequiredService<KeeperAssistantService>();

        try
        {
            return keeperService.GenerateRandomEventAsync(sceneType, dangerLevel);
        }
        catch (Exception ex)
        {
            return Task.FromResult($"❌ 生成隨機事件失敗: {ex.Message}");
        }
    }

    [McpServerTool, Description("戰鬥輔助工具")]
    public static string CombatAssistance(string weaponType, int characterDamageBonus = 0)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var keeperService = scope.ServiceProvider.GetRequiredService<KeeperAssistantService>();

        try
        {
            return keeperService.GenerateCombatAssistance(weaponType, characterDamageBonus);
        }
        catch (Exception ex)
        {
            return $"❌ 戰鬥輔助失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("快速骰子工具")]
    public static string QuickDiceRoll(string diceExpression)
    {
        try
        {
            var random = new Random();
            var result = 0;
            var description = "";

            // 解析骰子表達式
            if (diceExpression.ToLower().Contains("d"))
            {
                var parts = diceExpression.ToLower().Replace(" ", "").Split('d');
                if (parts.Length == 2 && int.TryParse(parts[0], out var count) && int.TryParse(parts[1], out var sides))
                {
                    var rolls = new List<int>();
                    for (int i = 0; i < count; i++)
                    {
                        var roll = random.Next(1, sides + 1);
                        rolls.Add(roll);
                        result += roll;
                    }
                    description = $"🎲 **{diceExpression}**: {string.Join(" + ", rolls)} = **{result}**";
                }
                else
                {
                    return "❌ 無效的骰子表達式格式";
                }
            }
            else if (int.TryParse(diceExpression, out var fixedValue))
            {
                result = fixedValue;
                description = $"🎲 固定值: **{result}**";
            }
            else
            {
                return "❌ 無法解析骰子表達式";
            }

            // 特殊結果提示
            var specialNote = "";
            if (diceExpression.ToLower().Contains("d100") || diceExpression.ToLower().Contains("d%"))
            {
                if (result <= 5) specialNote = " (大成功!)";
                else if (result >= 96) specialNote = " (大失敗!)";
            }

            return description + specialNote;
        }
        catch (Exception ex)
        {
            return $"❌ 骰子擲骰失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("獲取可用的角色職業模板")]
    public static async Task<string> GetAvailableCharacterTemplates()
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var templateService = scope.ServiceProvider.GetRequiredService<CharacterTemplateService>();

        try
        {
            var templates = await templateService.GetAvailableTemplatesAsync();

            var result = "🎭 **可選擇的角色職業**\n\n";

            foreach (var template in templates)
            {
                result += $"**{template.Occupation}**\n";
                result += $"📝 {template.Description}\n";
                result += $"🎂 推薦年齡: {template.AgeRange}\n";
                result += $"⚡ 特色: {template.RecommendedTraits}\n\n";
            }

            result += "💡 使用 `GenerateRandomCharacter` 工具並指定職業名稱來生成角色";

            return result;
        }
        catch (Exception ex)
        {
            return $"❌ 獲取職業模板失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("生成隨機角色供玩家選擇")]
    public static async Task<string> GenerateRandomCharacter(string occupation, string playerName)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var templateService = scope.ServiceProvider.GetRequiredService<CharacterTemplateService>();

        try
        {
            var config = await templateService.GenerateCharacterAsync(occupation);
            var character = await templateService.CreateCharacterFromConfigAsync(config, playerName);

            var result = $"✨ **角色生成成功！**\n\n";
            result += $"🎭 **{character.Name}**\n";
            result += $"👤 玩家: {character.PlayerName}\n";
            result += $"💼 職業: {character.Occupation}\n";
            result += $"🎂 年齡: {character.Age} | 🚻 性別: {character.Gender}\n";
            result += $"🏠 出生地: {character.Birthplace}\n";
            result += $"🆔 角色ID: {character.Id}\n\n";

            result += $"📊 **屬性值**\n";
            result += $"💪 力量: {character.Strength} | 🏃 敏捷: {character.Dexterity} | 🧠 智力: {character.Intelligence}\n";
            result += $"💫 意志: {character.Power} | 🛡️ 體質: {character.Constitution} | 📏 體型: {character.Size}\n";
            result += $"🌟 外貌: {character.Appearance} | 🎓 教育: {character.Education}\n\n";

            result += $"❤️ 生命值: {character.HitPoints} | 🔮 魔法值: {character.MagicPoints} | 🧠 理智值: {character.Sanity} | 🍀 幸運值: {character.Luck}\n\n";

            result += $"📚 **背景故事**\n{character.BackgroundStory}\n\n";
            result += $"👥 **重要之人**: {character.ImportantPerson}\n";
            result += $"💭 **思想信念**: {character.Ideology}\n";
            result += $"🏛️ **重要之地**: {character.SignificantLocation}\n";
            result += $"💎 **珍貴之物**: {character.TreasuredPossession}\n";
            result += $"🎭 **性格特質**: {character.Traits}\n\n";

            // 顯示主要專業技能
            if (config.ProfessionalSkillPoints.Any())
            {
                result += $"🎯 **主要專業技能**\n";
                foreach (var skill in config.ProfessionalSkillPoints.Take(5))
                {
                    var baseRate = await GetBaseSuccessRateAsync(skill.Key);
                    var totalValue = skill.Value + baseRate;
                    result += $"• {skill.Key}: {totalValue}%\n";
                }
            }

            result += $"\n✅ 角色已創建完成，可以開始遊戲了！";

            return result;
        }
        catch (Exception ex)
        {
            return $"❌ 生成角色失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("開始《沉眠之館》遊戲 - 自動初始化劇本和會話")]
    public static async Task<string> StartSleepingManorGame(int characterId, string keeperName = "AI Keeper")
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var scenarioService = scope.ServiceProvider.GetRequiredService<ScenarioService>();
        var characterService = scope.ServiceProvider.GetRequiredService<CharacterService>();
        var context = scope.ServiceProvider.GetRequiredService<TrpgDbContext>();

        try
        {
            // 獲取角色信息
            var character = await characterService.GetCharacterAsync(characterId);
            if (character == null)
                return "❌ 角色不存在，請先創建角色";

            // 檢查是否已有沉眠之館劇本
            var scenario = await context.Scenarios.FirstAsync();

            // 創建遊戲會話
            var sessionName = $"{character.Name}的沉眠之館冒險";
            var session = await scenarioService.CreateGameSessionAsync(scenario.Id, sessionName, keeperName);

            // 將角色加入會話
            var sessionCharacter = new SessionCharacter
            {
                GameSessionId = session.Id,
                PlayerCharacterId = character.Id,
                IsActive = true
            };
            context.SessionCharacters.Add(sessionCharacter);
            await context.SaveChangesAsync();

            var result = $"🎬 **《沉眠之館》冒險開始！**\n\n";
            result += $"🎭 調查員: **{character.Name}** ({character.Occupation})\n";
            result += $"🎮 會話ID: {session.Id}\n";
            result += $"👑 KP: {session.KeeperName}\n\n";

            result += $"📖 **劇本背景**\n";
            result += $"1926 年秋天，新英格蘭沿岸的空氣潮濕而冰冷。你收到了一封神秘的信件，信件的主人是考古學者亨利·阿什頓（Henry Ashton），他提到自己正在研究一棟被遺棄的古老莊園——布雷克伍德館（Blakewood Manor）。\n\n";

            result += $"他在信中提及「夢境與現實的交界」、「無法醒來的沉眠」以及「某種即將被喚醒的存在」。最後一句字跡潦草：「請來……在為時已晚之前。」\n\n";

            result += $"你帶著疑惑與不安，踏上了通往布雷克伍德莊園的小鎮。\n\n";

            result += $"🗺️ **當前位置**: 準備前往小鎮\n";
            result += $"🎯 **建議行動**: 你可以選擇先去小鎮酒館打聽消息，或者直接前往圖書館查閱資料\n\n";

            result += $"💡 **KP提示**: 使用 `GetSceneInfo` 工具來獲取場景詳情，使用 `GenerateSceneDescription` 來獲得豐富的場景描述";

            return result;
        }
        catch (Exception ex)
        {
            return $"❌ 開始遊戲失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("獲取遊戲進度建議 (KP輔助)")]
    public static async Task<string> GetGameProgressSuggestion(int sessionId, string currentSituation)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TrpgDbContext>();

        try
        {
            var session = await context.GameSessions
                .Include(s => s.Scenario)
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session == null)
                return "❌ 遊戲會話不存在";

            var suggestions = new List<string>();

            // 根據目前情況提供建議
            if (currentSituation.Contains("小鎮") || currentSituation.Contains("酒館") || currentSituation.Contains("圖書館"))
            {
                suggestions.Add("🏛️ **小鎮階段建議**:");
                suggestions.Add("• 在酒館與老闆湯姆·米勒對話，獲取布雷克伍德館的傳聞");
                suggestions.Add("• 在圖書館查閱當地歷史，了解布雷克伍德家族的過往");
                suggestions.Add("• 可進行【圖書館使用】或【快速交談】檢定獲得線索");
                suggestions.Add("• 準備前往莊園時，建議進行【準備工作】描述");
            }
            else if (currentSituation.Contains("莊園") || currentSituation.Contains("布雷克伍德"))
            {
                suggestions.Add("🏚️ **莊園探索階段建議**:");
                suggestions.Add("• 描述莊園的荒廢外觀和詭異氛圍");
                suggestions.Add("• 需要【力量】或【機械維修】檢定才能進入");
                suggestions.Add("• 進入後可探索客廳、書房、餐廳等房間");
                suggestions.Add("• 每個房間都有隱藏線索和可能的SAN檢定");
                suggestions.Add("• 建議晚上觸發夢境事件");
            }
            else if (currentSituation.Contains("夢境") || currentSituation.Contains("沉眠"))
            {
                suggestions.Add("💭 **夢境階段建議**:");
                suggestions.Add("• 描述現實與夢境的界線模糊");
                suggestions.Add("• 無面沉眠者可能出現，需要SAN檢定");
                suggestions.Add("• 角色可能聽到瑪莎的夢境低語");
                suggestions.Add("• 這是劇情轉折點，開始揭露真相");
            }

            suggestions.Add("");
            suggestions.Add("🎯 **通用KP技巧**:");
            suggestions.Add("• 使用【聆聽】檢定讓玩家聽到神秘聲音");
            suggestions.Add("• 用【偵查】檢定發現隱藏線索");
            suggestions.Add("• 適時進行【心理學】檢定理解NPC動機");
            suggestions.Add("• 營造緊張氛圍但不要過度使用SAN檢定");

            return string.Join("\n", suggestions);
        }
        catch (Exception ex)
        {
            return $"❌ 獲取進度建議失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("獲取沉眠之館劇情時間軸")]
    public static string GetSleepingManorTimeline()
    {
        var timeline = "📅 **《沉眠之館》劇情時間軸**\n\n";

        timeline += "**第一幕：小鎮導入** (30-45分鐘)\n";
        timeline += "• 玩家抵達小鎮，收集初步情報\n";
        timeline += "• 酒館：了解布雷克伍德館傳聞\n";
        timeline += "• 圖書館：查閱家族歷史和失蹤案件\n";
        timeline += "• 準備工作：購買補給品，決定行動時間\n\n";

        timeline += "**第二幕：莊園探索** (60-90分鐘)\n";
        timeline += "• 抵達莊園：外觀描述和進入檢定\n";
        timeline += "• 一樓探索：客廳、書房、餐廳、廚房\n";
        timeline += "• 二樓探索：瑪莎臥室、主臥室、閣樓\n";
        timeline += "• 收集日記、手稿和符號線索\n\n";

        timeline += "**第三幕：夢境侵蝕** (45-60分鐘)\n";
        timeline += "• 第一次夢境事件：無面沉眠者出現\n";
        timeline += "• 現實與夢境界線模糊\n";
        timeline += "• 瑪莎的夢境低語和線索拼湊\n";
        timeline += "• 發現地下室入口\n\n";

        timeline += "**第四幕：真相與決戰** (45-60分鐘)\n";
        timeline += "• 地下室儀式廳：血跡符號和祭壇\n";
        timeline += "• 儀式自動啟動：海水滲入\n";
        timeline += "• 最終選擇：封印、破壞或犧牲\n";
        timeline += "• 結局：成功、失敗或灰色結局\n\n";

        timeline += "**總預估時間：3-4小時**\n";
        timeline += "💡 **KP提示**: 可根據玩家節奏調整各幕時間長度";

        return timeline;
    }

    [McpServerTool, Description("獲取場景檢定建議")]
    public static async Task<string> GetSceneRollSuggestions(string sceneName, string playerAction)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TrpgDbContext>();

        try
        {
            var result = $"🎲 **{sceneName}場景檢定建議**\n\n";

            // 從資料庫獲取該場景的檢定建議
            var sceneSuggestions = await context.SceneRollSuggestions
                .Where(srs => srs.SceneName == sceneName && srs.IsActive)
                .OrderBy(srs => srs.DisplayOrder)
                .ToListAsync();

            if (sceneSuggestions.Any())
            {
                result += "**場景特定檢定**:\n";
                foreach (var suggestion in sceneSuggestions)
                {
                    result += $"• {suggestion.SuggestionDescription}\n";
                }
                result += "\n";
            }

            // 根據玩家行動提供建議
            if (!string.IsNullOrEmpty(playerAction))
            {
                var actionSuggestions = await context.ActionSuggestions
                    .Where(a => a.IsActive && playerAction.Contains(a.ActionKeyword))
                    .OrderBy(a => a.DisplayOrder)
                    .ToListAsync();

                if (actionSuggestions.Any())
                {
                    result += $"**針對玩家行動 \"{playerAction}\" 的建議**:\n";
                    foreach (var suggestion in actionSuggestions)
                    {
                        result += $"• {suggestion.SuggestionDescription}\n";
                    }
                    result += "\n";
                }
            }

            result += "💡 **檢定難度參考**:\n";
            result += "• 普通難度: 無修正\n";
            result += "• 困難: -20修正\n";
            result += "• 極難: -40修正\n";

            return result;
        }
        catch (Exception ex)
        {
            return $"❌ 獲取場景檢定建議失敗: {ex.Message}";
        }
    }

    [McpServerTool, Description("獲取NPC反應建議")]
    public static async Task<string> GetNpcReactionSuggestion(string npcName, string playerApproach)
    {
        if (_serviceProvider == null) return "服務未初始化";

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TrpgDbContext>();

        try
        {
            var result = $"🎭 **{npcName} 的反應建議**\n\n";

            // 從資料庫獲取該NPC的反應資料
            var npcReactions = await context.NpcReactions
                .Where(nr => nr.NpcName == npcName && nr.IsActive)
                .OrderBy(nr => nr.DisplayOrder)
                .ToListAsync();

            if (npcReactions.Any())
            {
                result += "**根據玩家態度的反應**:\n";
                foreach (var reaction in npcReactions)
                {
                    result += $"**{reaction.PlayerApproach}態度**: {reaction.ReactionDescription}\n\n";
                }
            }
            else
            {
                result += "**通用NPC反應原則**:\n";
                result += "• 友善態度: NPC更願意分享資訊和提供幫助\n";
                result += "• 威脅態度: NPC變得防禦性，可能隱藏重要資訊\n";
                result += "• 專業態度: 適合學術型NPC，能獲得更深入的知識\n";
                result += "• 同情態度: 對受到創傷的NPC有效，能獲得情感上的連結\n\n";
            }

            if (!string.IsNullOrEmpty(playerApproach))
            {
                result += $"**針對玩家採取的 \"{playerApproach}\" 方式**:\n";
                result += "建議進行適當的社交技能檢定:\n";
                result += "• 【快速交談】- 快速建立關係\n";
                result += "• 【說服】- 讓NPC接受你的觀點\n";
                result += "• 【心理學】- 理解NPC的真實想法\n";
                result += "• 【恐嚇】- 強迫獲取資訊(有風險)\n";
            }

            return result;
        }
        catch (Exception ex)
        {
            return $"❌ 獲取NPC反應建議失敗: {ex.Message}";
        }
    }

    // 輔助方法
    private static async Task<int> GetBaseSuccessRateAsync(string skillName)
    {
        if (_serviceProvider == null) return 0;

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TrpgDbContext>();

        try
        {
            var basicSkill = await context.BasicSkills
                .FirstOrDefaultAsync(bs => bs.Name == skillName);

            return basicSkill?.BaseSuccessRate ?? 0;
        }
        catch
        {
            return 0;
        }
    }
}