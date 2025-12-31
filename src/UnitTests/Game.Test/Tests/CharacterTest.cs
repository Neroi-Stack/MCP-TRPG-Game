using Game.Service.Data;
using Game.Service.Data.Models;
using Game.Service.Request;
using Game.Service.Services;
using Microsoft.EntityFrameworkCore;
namespace Game.Test.Tests
{
    public class CharacterTest
    {
        [Fact]
        public async Task GetAllCharactersAsync_ReturnFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CharacterService(context);

            // Act
            var result = await service.GetAllCharactersAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task CreateCharacterAsync_And_GetCharacterByIdAsync()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CharacterService(context);
            var request = new PlayerCharacterRequest
            {
                Name = "Test",
                Gender = "M",
                Age = 20,
                PhysicalDesc = "Desc",
                Biography = "Bio",
                StatusEffects = "None",
                Notes = "Note",
                IsDead = false,
                IsTemplate = false,
                IsActive = true
            };

            // Act
            var created = await service.CreateCharacterAsync(request);
            var fetched = await service.GetCharacterByIdAsync(created!.Id);

            // Assert
            Assert.NotNull(created);
            Assert.NotNull(fetched);
            Assert.Equal("Test", fetched.Name);
        }

        [Fact]
        public async Task UpdateCharacterAsync_UpdatesFields()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CharacterService(context);
            var request = new PlayerCharacterRequest { Name = "Old", Gender = "M", Age = 20, IsActive = true };

            // Act
            var created = await service.CreateCharacterAsync(request);
            var update = new PlayerCharacterRequest { Name = "New", Gender = "F", Age = 30, IsActive = false };
            var updated = await service.UpdateCharacterAsync(created!.Id, update);

            // Assert
            Assert.NotNull(created);
            Assert.NotNull(updated);
            Assert.Equal("New", updated.Name);
            Assert.Equal("F", updated.Gender);
            Assert.Equal(30, updated.Age);
            Assert.False(updated.IsActive);
        }

        [Fact]
        public async Task DeleteCharacterAsync_RemovesCharacter()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CharacterService(context);
            var request = new PlayerCharacterRequest { Name = "DeleteMe", Gender = "M", Age = 20, IsActive = true };
            var created = await service.CreateCharacterAsync(request);

            // Act
            var deleted = await service.DeleteCharacterAsync(created!.Id);
            var fetched = await service.GetCharacterByIdAsync(created.Id);

            // Assert
            Assert.NotNull(created);
            Assert.True(deleted);
            Assert.Null(fetched);
        }

        [Fact]
        public async Task UpdateCharacterAttributeAsync_UpdatesValue()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CharacterService(context);
            var attr = new Attributes { Name = "HP", Description = "Health" };
            context.Attributes.Add(attr);
            await context.SaveChangesAsync();
            var request = new PlayerCharacterRequest { Name = "AttrTest", Gender = "M", Age = 20, IsActive = true };
            var created = await service.CreateCharacterAsync(request);
            Assert.NotNull(created);
            var charAttr = new CharacterAttribute { CharacterId = created.Id, AttributeId = attr.Id, MaxValue = 100, CurrentValue = 50 };
            context.CharacterAttributes.Add(charAttr);
            await context.SaveChangesAsync();

            // Act
            var updated = await service.UpdateCharacterAttributeAsync(created.Id, "HP", 80);
            var fetched = await service.GetCharacterByIdAsync(created.Id);
            var hp = fetched!.CharacterAttributes.FirstOrDefault(a => a != null && a.Attribute != null && a.Attribute.Name == "HP");

            // Assert
            Assert.NotNull(updated);
            Assert.NotNull(fetched);
            Assert.NotNull(hp);
            Assert.NotNull(hp.Attribute);
            Assert.Equal(80, hp.CurrentValue);
        }

        [Fact]
        public async Task CreateCharacterFromTemplateIdAsync_CopiesTemplate()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CharacterService(context);
            var attr = new Attributes { Name = "HP", Description = "Health" };
            var skill = new Skill { Name = "Sword", Description = "Sword skill", Category = "Combat", BaseSuccessRate = 50, IsBasic = true, IsActive = true, DisplayOrder = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var item = new Item { Name = "Potion", Description = "Heals HP", Category = "Consumable", Stats = "Heal:50", OwnerNotes = "", Weight = 0.1M, IsConsumable = true, IsCursed = false, IsActive = true, DisplayOrder = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            context.Attributes.Add(attr);
            context.Skills.Add(skill);
            context.Items.Add(item);
            await context.SaveChangesAsync();
            var template = new PlayerCharacter { Name = "Template", Gender = "F", Age = 25, IsTemplate = true, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            context.PlayerCharacters.Add(template);
            await context.SaveChangesAsync();
            var charAttr = new CharacterAttribute { CharacterId = template.Id, AttributeId = attr.Id, MaxValue = 100, CurrentValue = 100 };
            var charSkill = new CharacterSkill { CharacterId = template.Id, SkillId = skill.Id, Proficiency = 80 };
            var charItem = new CharacterItem { CharacterId = template.Id, ItemId = item.Id, Quantity = 2 };
            context.CharacterAttributes.Add(charAttr);
            context.CharacterSkills.Add(charSkill);
            context.CharacterItems.Add(charItem);
            await context.SaveChangesAsync();

            // Act
            var copied = await service.CreateCharacterFromTemplateIdAsync(template.Id);

            // Assert
            Assert.NotNull(copied);
            Assert.Equal("Template", copied.Name);
            Assert.False(copied.IsTemplate);
            Assert.Single(copied.CharacterAttributes);
            Assert.Single(copied.CharacterSkills);
            Assert.Single(copied.CharacterItems);
        }

        [Fact]
        public async Task UpdateCharacterAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CharacterService(context);
            var update = new PlayerCharacterRequest { Name = "X", Gender = "F", Age = 1, IsActive = false };

            // Act
            var result = await service.UpdateCharacterAsync(999, update);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateCharacterAttributeAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CharacterService(context);

            // Act
            var result = await service.UpdateCharacterAttributeAsync(999, "NotExist", 10);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteCharacterAsync_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CharacterService(context);

            // Act
            var result = await service.DeleteCharacterAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CreateCharacterFromTemplateIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new CharacterService(context);

            // Act
            var result = await service.CreateCharacterFromTemplateIdAsync(999);

            // Assert
            Assert.Null(result);
        }
    }
}