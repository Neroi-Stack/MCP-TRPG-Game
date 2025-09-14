using MCPTRPGGame.Models;

namespace MCPTRPGGame.Services;

/// <summary>
/// CoC7 規則相關的共用服務
/// </summary>
public class Coc7RulesService
{
    private readonly Random _random = new();

    public int Rolld4(int times)
    {
        int total = 0;
        for (int i = 0; i < times; i++)
        {
            total += _random.Next(1, 5);
        }
        return total;
    }

    public int Rolld6(int times)
    {
        int total = 0;
        for (int i = 0; i < times; i++)
        {
            total += _random.Next(1, 7);
        }
        return total;
    }

    public int Rolld8(int times)
    {
        int total = 0;
        for (int i = 0; i < times; i++)
        {
            total += _random.Next(1, 9);
        }
        return total;
    }

    public int Rolld12(int times)
    {
        int total = 0;
        for (int i = 0; i < times; i++)
        {
            total += _random.Next(1, 13);
        }
        return total;
    }

    public int Rolld20(int times)
    {
        int total = 0;
        for (int i = 0; i < times; i++)
        {
            total += _random.Next(1, 21);
        }
        return total;
    }

    public int Rolld100(int times)
    {
        int total = 0;
        for (int i = 0; i < times; i++)
        {
            var tensdigits = _random.Next(0, 10);
            var onesdigits = _random.Next(0, 10);
            int result = tensdigits * 10 + onesdigits;
            if (result == 0) result = 100;
            total += result;
        }
        return total;
    }

    /// <summary>
    /// 使用 CoC7 標準方法生成隨機屬性
    /// </summary>
    public void GenerateRandomAttributes(PlayerCharacter character)
    {
        // STR, DEX, CON, APP, POW, SIZ = 3d6 × 5 (15-90)
        character.Strength = Rolld6(3) * 5;
        character.Dexterity = Rolld6(3) * 5;
        character.Constitution = Rolld6(3) * 5;
        character.Appearance = Rolld6(3) * 5;
        character.Power = Rolld6(3) * 5;
        character.Size = Rolld6(3) * 5;

        // INT, EDU = (2d6 + 6) × 5 (40-90)
        character.Intelligence = (Rolld6(2) + 6) * 5;
        character.Education = (Rolld6(2) + 6) * 5;

        // 幸運值 = 3d6 × 5 (15-90)
        character.Luck = Rolld6(3) * 5;
    }

    /// <summary>
    /// 應用年齡修正到角色
    /// </summary>
    public void ApplyAgeModifiers(PlayerCharacter character)
    {
        switch (character.Age)
        {
            case >= 15 and <= 19:
                // 青少年：-5 STR, -5 SIZ, +5 EDU, +10 幸運檢定次數
                character.Strength = Math.Max(character.Strength - 5, 1);
                character.Size = Math.Max(character.Size - 5, 1);
                character.Education = Math.Max(character.Education - 5, 1);
                // 可進行兩次幸運檢定，取較高值
                var luck1 = Rolld6(3) * 5;
                var luck2 = Rolld6(3) * 5;
                character.Luck = Math.Max(luck1, luck2);
                break;

            case >= 20 and <= 39:
                // 青年：可進行一次教育檢定來提升
                if (_random.Next(1, 101) <= character.Education)
                {
                    character.Education = Math.Min(character.Education + _random.Next(1, 11), 99);
                }
                break;

            case >= 40 and <= 49:
                // 中年：可進行兩次教育檢定，-5 STR/CON/DEX之一，-5 APP
                for (int i = 0; i < 2; i++)
                {
                    if (_random.Next(1, 101) <= character.Education)
                    {
                        character.Education = Math.Min(character.Education + _random.Next(1, 11), 99);
                    }
                }

                // 隨機減少 STR/CON/DEX 其中之一
                var physicalAttributes = new[] { "STR", "CON", "DEX" };
                var selectedAttribute = physicalAttributes[_random.Next(physicalAttributes.Length)];
                switch (selectedAttribute)
                {
                    case "STR": character.Strength = Math.Max(character.Strength - 5, 1); break;
                    case "CON": character.Constitution = Math.Max(character.Constitution - 5, 1); break;
                    case "DEX": character.Dexterity = Math.Max(character.Dexterity - 5, 1); break;
                }
                character.Appearance = Math.Max(character.Appearance - 5, 1);
                break;

            case >= 50 and <= 59:
                // 中老年：可進行三次教育檢定，-10 STR/CON/DEX之一，-10 APP
                for (int i = 0; i < 3; i++)
                {
                    if (_random.Next(1, 101) <= character.Education)
                    {
                        character.Education = Math.Min(character.Education + _random.Next(1, 11), 99);
                    }
                }

                var physicalAttributes2 = new[] { "STR", "CON", "DEX" };
                var selectedAttribute2 = physicalAttributes2[_random.Next(physicalAttributes2.Length)];
                switch (selectedAttribute2)
                {
                    case "STR": character.Strength = Math.Max(character.Strength - 10, 1); break;
                    case "CON": character.Constitution = Math.Max(character.Constitution - 10, 1); break;
                    case "DEX": character.Dexterity = Math.Max(character.Dexterity - 10, 1); break;
                }
                character.Appearance = Math.Max(character.Appearance - 10, 1);
                break;

            case >= 60 and <= 69:
                // 老年：可進行四次教育檢定，-20 STR/CON/DEX之一，-15 APP
                for (int i = 0; i < 4; i++)
                {
                    if (_random.Next(1, 101) <= character.Education)
                    {
                        character.Education = Math.Min(character.Education + _random.Next(1, 11), 99);
                    }
                }

                var physicalAttributes3 = new[] { "STR", "CON", "DEX" };
                var selectedAttribute3 = physicalAttributes3[_random.Next(physicalAttributes3.Length)];
                switch (selectedAttribute3)
                {
                    case "STR": character.Strength = Math.Max(character.Strength - 20, 1); break;
                    case "CON": character.Constitution = Math.Max(character.Constitution - 20, 1); break;
                    case "DEX": character.Dexterity = Math.Max(character.Dexterity - 20, 1); break;
                }
                character.Appearance = Math.Max(character.Appearance - 15, 1);
                break;

            case >= 70 and <= 79:
                // 高齡：可進行四次教育檢定，-40 STR/CON/DEX之一，-20 APP
                for (int i = 0; i < 4; i++)
                {
                    if (_random.Next(1, 101) <= character.Education)
                    {
                        character.Education = Math.Min(character.Education + _random.Next(1, 11), 99);
                    }
                }

                var physicalAttributes4 = new[] { "STR", "CON", "DEX" };
                var selectedAttribute4 = physicalAttributes4[_random.Next(physicalAttributes4.Length)];
                switch (selectedAttribute4)
                {
                    case "STR": character.Strength = Math.Max(character.Strength - 40, 1); break;
                    case "CON": character.Constitution = Math.Max(character.Constitution - 40, 1); break;
                    case "DEX": character.Dexterity = Math.Max(character.Dexterity - 40, 1); break;
                }
                character.Appearance = Math.Max(character.Appearance - 20, 1);
                break;

            case >= 80:
                // 極高齡：可進行四次教育檢定，-80 STR/CON/DEX之一，-25 APP
                for (int i = 0; i < 4; i++)
                {
                    if (_random.Next(1, 101) <= character.Education)
                    {
                        character.Education = Math.Min(character.Education + _random.Next(1, 11), 99);
                    }
                }

                var physicalAttributes5 = new[] { "STR", "CON", "DEX" };
                var selectedAttribute5 = physicalAttributes5[_random.Next(physicalAttributes5.Length)];
                switch (selectedAttribute5)
                {
                    case "STR": character.Strength = Math.Max(character.Strength - 80, 1); break;
                    case "CON": character.Constitution = Math.Max(character.Constitution - 80, 1); break;
                    case "DEX": character.Dexterity = Math.Max(character.Dexterity - 80, 1); break;
                }
                character.Appearance = Math.Max(character.Appearance - 25, 1);
                break;
        }
    }

    /// <summary>
    /// 應用年齡修正到生成配置
    /// </summary>
    public void ApplyAgeModifiersToConfig(GeneratedCharacterConfig config)
    {
        switch (config.Age)
        {
            case >= 15 and <= 19:
                // 青少年：-5 STR, -5 SIZ, -5 EDU
                config.Strength = Math.Max(config.Strength - 5, 1);
                config.Size = Math.Max(config.Size - 5, 1);
                config.Education = Math.Max(config.Education - 5, 1);
                break;

            case >= 40 and <= 49:
                // 中年：可進行兩次教育檢定，-5 STR/CON/DEX之一，-5 APP
                for (int i = 0; i < 2; i++)
                {
                    if (_random.Next(1, 101) <= config.Education)
                    {
                        config.Education = Math.Min(config.Education + _random.Next(1, 11), 99);
                    }
                }

                // 隨機減少一個體能屬性
                var physicalReduction = _random.Next(1, 4);
                switch (physicalReduction)
                {
                    case 1: config.Strength = Math.Max(config.Strength - 5, 1); break;
                    case 2: config.Constitution = Math.Max(config.Constitution - 5, 1); break;
                    case 3: config.Dexterity = Math.Max(config.Dexterity - 5, 1); break;
                }
                config.Appearance = Math.Max(config.Appearance - 5, 1);
                break;

            case >= 50 and <= 59:
                // 中老年：可進行三次教育檢定，-10 STR/CON/DEX之一，-10 APP
                for (int i = 0; i < 3; i++)
                {
                    if (_random.Next(1, 101) <= config.Education)
                    {
                        config.Education = Math.Min(config.Education + _random.Next(1, 11), 99);
                    }
                }

                var physicalReduction2 = _random.Next(1, 4);
                switch (physicalReduction2)
                {
                    case 1: config.Strength = Math.Max(config.Strength - 10, 1); break;
                    case 2: config.Constitution = Math.Max(config.Constitution - 10, 1); break;
                    case 3: config.Dexterity = Math.Max(config.Dexterity - 10, 1); break;
                }
                config.Appearance = Math.Max(config.Appearance - 10, 1);
                break;

            case >= 60 and <= 69:
                // 老年：可進行四次教育檢定，-20 STR/CON/DEX之一，-15 APP
                for (int i = 0; i < 4; i++)
                {
                    if (_random.Next(1, 101) <= config.Education)
                    {
                        config.Education = Math.Min(config.Education + _random.Next(1, 11), 99);
                    }
                }

                var physicalReduction3 = _random.Next(1, 4);
                switch (physicalReduction3)
                {
                    case 1: config.Strength = Math.Max(config.Strength - 20, 1); break;
                    case 2: config.Constitution = Math.Max(config.Constitution - 20, 1); break;
                    case 3: config.Dexterity = Math.Max(config.Dexterity - 20, 1); break;
                }
                config.Appearance = Math.Max(config.Appearance - 15, 1);
                break;

            case >= 70 and <= 79:
                // 高齡：可進行四次教育檢定，-40 STR/CON/DEX之一，-20 APP
                for (int i = 0; i < 4; i++)
                {
                    if (_random.Next(1, 101) <= config.Education)
                    {
                        config.Education = Math.Min(config.Education + _random.Next(1, 11), 99);
                    }
                }

                var physicalReduction4 = _random.Next(1, 4);
                switch (physicalReduction4)
                {
                    case 1: config.Strength = Math.Max(config.Strength - 40, 1); break;
                    case 2: config.Constitution = Math.Max(config.Constitution - 40, 1); break;
                    case 3: config.Dexterity = Math.Max(config.Dexterity - 40, 1); break;
                }
                config.Appearance = Math.Max(config.Appearance - 20, 1);
                break;

            case >= 80:
                // 極高齡：可進行四次教育檢定，-80 STR/CON/DEX之一，-25 APP
                for (int i = 0; i < 4; i++)
                {
                    if (_random.Next(1, 101) <= config.Education)
                    {
                        config.Education = Math.Min(config.Education + _random.Next(1, 11), 99);
                    }
                }

                var physicalReduction5 = _random.Next(1, 4);
                switch (physicalReduction5)
                {
                    case 1: config.Strength = Math.Max(config.Strength - 80, 1); break;
                    case 2: config.Constitution = Math.Max(config.Constitution - 80, 1); break;
                    case 3: config.Dexterity = Math.Max(config.Dexterity - 80, 1); break;
                }
                config.Appearance = Math.Max(config.Appearance - 25, 1);
                break;
        }
    }

    /// <summary>
    /// 計算衍生屬性
    /// </summary>
    public void CalculateDerivedAttributes(PlayerCharacter character)
    {
        // 生命值 = (體質 + 體型) / 10
        character.HitPoints = (character.Constitution + character.Size) / 10;
        character.CurrentHitPoints = character.HitPoints;

        // 魔法值 = 意志 / 5
        character.MagicPoints = character.Power / 5;
        character.CurrentMagicPoints = character.MagicPoints;

        // 理智值 = 意志
        character.Sanity = character.Power;
        character.CurrentSanity = character.Sanity;

        // 幸運值只在未設定時生成 (3d6 × 5)
        if (character.Luck == 0)
        {
            character.Luck = Rolld6(3) * 5;
        }
        character.CurrentLuck = character.Luck;
    }
}