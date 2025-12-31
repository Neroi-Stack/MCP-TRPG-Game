using Game.Service.Data;
using Game.Service.Data.Models;
using Game.Service.Services;
using Microsoft.EntityFrameworkCore;

namespace Game.Test.Tests
{
    public class SenarioTest
    {
        [Fact]
        public async Task GetScenarioByIdAsync_ReturnsScenario_WhenExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            context.Scenarios.Add(new Scenario { Id = 2, Name = "S2", Description = "D2", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            await context.SaveChangesAsync();
            var service = new ScenarioService(context);

            // Act
            var result = await service.GetScenarioByIdAsync(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("S2", result.Name);
        }

        [Fact]
        public async Task GetAllScenariosAsync_ReturnsScenarios()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            context.Scenarios.Add(new Scenario { Name = "S1", Description = "D1", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
            await context.SaveChangesAsync();
            var service = new ScenarioService(context);

            // Act
            var result = await service.GetAllScenariosAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("S1", result[0]?.Name);
        }

        [Fact]
        public async Task GetScenarioByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrpgDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            using var context = new TrpgDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var service = new ScenarioService(context);

            // Act
            var result = await service.GetScenarioByIdAsync(999);

            // Assert
            Assert.Null(result);
        }
    }
}
