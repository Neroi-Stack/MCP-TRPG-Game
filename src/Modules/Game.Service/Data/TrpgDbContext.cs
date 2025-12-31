using Microsoft.EntityFrameworkCore;
using Game.Service.Data.Models;

namespace Game.Service.Data;

/// <summary>
/// Entity Framework Core DB context for the TRPG game.
/// Contains DbSet properties for each entity and fluent configuration
/// for join tables and composite keys in OnModelCreating.
/// </summary>
public class TrpgDbContext(DbContextOptions<TrpgDbContext> options) : DbContext(options)
{
	// Core entities
	public DbSet<PlayerCharacter> PlayerCharacters { get; set; }
	public DbSet<NonPlayerCharacter> NonPlayerCharacters { get; set; }
	public DbSet<Skill> Skills { get; set; }
	public DbSet<Attributes> Attributes { get; set; }
	public DbSet<Item> Items { get; set; }
	public DbSet<Scenario> Scenarios { get; set; }
	public DbSet<Scene> Scenes { get; set; }
	public DbSet<RandomEvent> RandomEvents { get; set; }
	public DbSet<RandomElement> RandomElements { get; set; }
	public DbSet<EventIntensity> EventIntensities { get; set; }
	public DbSet<GameRecords> GameRecords { get; set; }
	public DbSet<ActionSuggestion> ActionSuggestions { get; set; }
	public DbSet<NpcReaction> NpcReactions { get; set; }
	public DbSet<SceneRollSuggestion> SceneRollSuggestions { get; set; }
	public DbSet<CheckRequirement> CheckRequirements { get; set; }

	// Join / linking entities (many-to-many explicit tables)
	public DbSet<CharacterSkill> CharacterSkills { get; set; }
	public DbSet<CharacterItem> CharacterItems { get; set; }
	public DbSet<CharacterAttribute> CharacterAttributes { get; set; }
	public DbSet<ScenarioCharacter> ScenarioCharacters { get; set; }
	public DbSet<RandomEventElement> RandomEventElements { get; set; }
	public DbSet<SceneItem> SceneItems { get; set; }
	public DbSet<SceneActionSuggestion> SceneActionSuggestions { get; set; }
	public DbSet<CharacterActionSuggestion> CharacterActionSuggestions { get; set; }
	public DbSet<ActionSuggestionNpcReaction> ActionSuggestionNpcReactions { get; set; }
	public DbSet<SceneRollSuggestionScene> SceneRollSuggestionScenes { get; set; }
	public DbSet<SceneRollSuggestionSkill> SceneRollSuggestionSkills { get; set; }

	// TODO: CREATE Character flow support
	public DbSet<Profession> Professions { get; set; }
	public DbSet<StatusEffect> StatusEffects { get; set; }
	public DbSet<RollHistory> RollHistories { get; set; }
	public DbSet<CombatSession> CombatSessions { get; set; }
	public DbSet<CombatAction> CombatActions { get; set; }
	public DbSet<CharacterStatusEffect> CharacterStatusEffects { get; set; }
	public DbSet<ProfessionSkill> ProfessionSkills { get; set; }

	/// <summary>
	/// Configure composite keys and relationships for many-to-many join tables.
	/// Keep mappings explicit to ensure correct composite primary keys and indexes.
	/// </summary>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrpgDbContext).Assembly);
		base.OnModelCreating(modelBuilder);
	}

	public override int SaveChanges()
	{
		// 確保外鍵約束已啟用
		Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON;");
		return base.SaveChanges();
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		// 確保外鍵約束已啟用
		await Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = ON;", cancellationToken);
		return await base.SaveChangesAsync(cancellationToken);
	}
}