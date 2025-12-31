using Game.Service.Data;
using Game.Service.Data.Models;
using Game.Service.Services;
using Microsoft.EntityFrameworkCore;
namespace Game.Test.Tests
{
    public class CheckTest
    {
        public CheckTest()
        {
            
        }
        [Fact]
        public async Task RollDiceAsync_ReturnsValue()
        {
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CheckService(context);

            var result = await service.RollDiceAsync("2d6");
            Assert.InRange(result, 2, 12);
        }

        [Fact]
        public async Task RollDiceAsync_Throws_OnInvalidFormat()
        {
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CheckService(context);

            await Assert.ThrowsAsync<ArgumentException>(() => service.RollDiceAsync("badformat"));
        }

        [Fact]
        public async Task SanityCheckAsync_NoAttribute_ReturnsMessage()
        {
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CheckService(context);

            var msg = await service.SanityCheckAsync(1, "1d6");
            Assert.Contains("No sanity attribute", msg);
        }

        [Fact]
        public async Task SanityCheckAsync_WithAttribute_ReturnsResult()
        {
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CheckService(context);


            // Arrange
            var player = new PlayerCharacter { Id = 1, Name = "Test" };
            context.PlayerCharacters.Add(player);
            var attr = new Attributes { Name = "Sanity", Description = "San" };
            context.Attributes.Add(attr);
            await context.SaveChangesAsync();
            var charAttr = new CharacterAttribute { CharacterId = 1, AttributeId = attr.Id, MaxValue = 100, CurrentValue = 50 };
            context.CharacterAttributes.Add(charAttr);
            await context.SaveChangesAsync();

            var msg = await service.SanityCheckAsync(1, "1d6");
            Assert.Contains("Roll:", msg);
            Assert.Contains("Threshold: 50", msg);
        }

        [Fact]
        public async Task AttributeCheckAsync_ByName_And_ById()
        {
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CheckService(context);


            // Arrange
            var player = new PlayerCharacter { Id = 1, Name = "Test" };
            context.PlayerCharacters.Add(player);
            var attr = new Attributes { Name = "HP", Description = "Health" };
            context.Attributes.Add(attr);
            await context.SaveChangesAsync();
            var charAttr = new CharacterAttribute { CharacterId = 1, AttributeId = attr.Id, MaxValue = 100, CurrentValue = 60 };
            context.CharacterAttributes.Add(charAttr);
            await context.SaveChangesAsync();

            var msgByName = await service.AttributeCheckAsync(1, "HP", "1d6");
            Assert.Contains("Roll:", msgByName);
            Assert.Contains("Threshold: 60", msgByName);

            var msgById = await service.AttributeCheckAsync(1, attr.Id.ToString(), "1d6");
            Assert.Contains("Roll:", msgById);
            Assert.Contains("Threshold: 60", msgById);
        }

        [Fact]
        public async Task AttributeCheckAsync_NotFound()
        {
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CheckService(context);

            var msg = await service.AttributeCheckAsync(1, "STR", "1d6");
            Assert.Contains("not found", msg);
        }

        [Fact]
        public async Task SkillCheckAsync_ByName_And_ById()
        {
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CheckService(context);


            // Arrange
            var player = new PlayerCharacter { Id = 1, Name = "Test" };
            context.PlayerCharacters.Add(player);
            var skill = new Skill { Name = "Sword", Description = "Sword skill", BaseSuccessRate = 30, IsBasic = true, IsActive = true, DisplayOrder = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            context.Skills.Add(skill);
            await context.SaveChangesAsync();
            var charSkill = new CharacterSkill { CharacterId = 1, SkillId = skill.Id, Proficiency = 20 };
            context.CharacterSkills.Add(charSkill);
            await context.SaveChangesAsync();

            var msgByName = await service.SkillCheckAsync(1, "Sword", "1d6");
            Assert.Contains("Roll:", msgByName);
            Assert.Contains("Target: 50", msgByName);

            var msgById = await service.SkillCheckAsync(1, skill.Id.ToString(), "1d6");
            Assert.Contains("Roll:", msgById);
            Assert.Contains("Target: 50", msgById);
        }

        [Fact]
        public async Task SkillCheckAsync_NotFound()
        {
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CheckService(context);

            var msg = await service.SkillCheckAsync(1, "Magic", "1d6");
            Assert.Contains("not found", msg);
        }

        [Fact]
        public async Task SavingThrowAsync_DelegatesToAttributeCheck()
        {
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CheckService(context);


            // Arrange
            var player = new PlayerCharacter { Id = 1, Name = "Test" };
            context.PlayerCharacters.Add(player);
            var attr = new Attributes { Name = "Luck", Description = "Luck" };
            context.Attributes.Add(attr);
            await context.SaveChangesAsync();
            var charAttr = new CharacterAttribute { CharacterId = 1, AttributeId = attr.Id, MaxValue = 100, CurrentValue = 70 };
            context.CharacterAttributes.Add(charAttr);
            await context.SaveChangesAsync();

            var msg = await service.SavingThrowAsync(1, "Luck", "1d6");
            Assert.Contains("Roll:", msg);
            Assert.Contains("Threshold: 70", msg);
        }

        [Fact]
        public async Task CalculateDamageAsync_UsesItemStats()
        {
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CheckService(context);

            var item = new Item { Name = "Sword", Description = "Sword", Stats = "2d6", Category = "Weapon", OwnerNotes = "", Weight = 1, IsConsumable = false, IsCursed = false, IsActive = true, DisplayOrder = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            context.Items.Add(item);
            await context.SaveChangesAsync();

            var msg = await service.CalculateDamageAsync(1, "Sword", "1d4");
            Assert.Contains("Damage:", msg);
            Assert.Contains("dice: 2d6", msg);
        }

        [Fact]
        public async Task CalculateDamageAsync_UsesRollExpression()
        {
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CheckService(context);

            var msg = await service.CalculateDamageAsync(1, "Unknown", "1d4");
            Assert.Contains("Damage:", msg);
            Assert.Contains("dice: 1d4", msg);
        }

        [Fact]
        public async Task AutoRollPlayerAttributeAsync_AssignsAttributes()
        {
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CheckService(context);

            // Arrange
            var player = new PlayerCharacter { Id = 1, Name = "Test" };
            context.PlayerCharacters.Add(player);
            context.Attributes.Add(new Attributes { Name = "POW", Description = "Power" });
            context.Attributes.Add(new Attributes { Name = "SAN", Description = "Sanity" });
            context.Attributes.Add(new Attributes { Name = "SIZ", Description = "Size" });
            context.Attributes.Add(new Attributes { Name = "INT", Description = "Intelligence" });
            context.Attributes.Add(new Attributes { Name = "EDU", Description = "Education" });
            context.Attributes.Add(new Attributes { Name = "STR", Description = "Strength" });
            await context.SaveChangesAsync();

            // Act
            var msg = await service.AutoRollPlayerAttributeAsync(1);

            // Assert
            Assert.Equal("Attributes rolled and assigned successfully.", msg);
            var attrs = context.CharacterAttributes.Where(a => a.CharacterId == 1).ToList();
            Assert.Equal(6, attrs.Count);
        }
    }
}