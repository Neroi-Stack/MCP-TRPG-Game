using Microsoft.EntityFrameworkCore;
using MCPTRPGGame.Models;

namespace MCPTRPGGame.Data;

/// <summary>
/// TRPG 資料庫上下文
/// </summary>
public class TrpgDbContext : DbContext
{
    public TrpgDbContext(DbContextOptions<TrpgDbContext> options) : base(options)
    {
    }

    // 角色相關
    public DbSet<PlayerCharacter> PlayerCharacters { get; set; }
    public DbSet<NonPlayerCharacter> NonPlayerCharacters { get; set; }
    public DbSet<CharacterLog> CharacterLogs { get; set; }
    public DbSet<CharacterTemplate> CharacterTemplates { get; set; }

    // 技能相關
    public DbSet<Skill> Skills { get; set; }
    public DbSet<BasicSkill> BasicSkills { get; set; }
    public DbSet<CharacterSkill> CharacterSkills { get; set; }
    public DbSet<NpcSkill> NpcSkills { get; set; }

    // 物品相關
    public DbSet<Item> Items { get; set; }
    public DbSet<CharacterItem> CharacterItems { get; set; }
    public DbSet<SceneItem> SceneItems { get; set; }

    // 場景和劇本
    public DbSet<Scene> Scenes { get; set; }
    public DbSet<Scenario> Scenarios { get; set; }

    // 遊戲會話
    public DbSet<GameSession> GameSessions { get; set; }
    public DbSet<SessionCharacter> SessionCharacters { get; set; }

    // 記錄系統
    public DbSet<RollRecord> RollRecords { get; set; }
    public DbSet<SanityRecord> SanityRecords { get; set; }
    public DbSet<GameLog> GameLogs { get; set; }

    // 系統表格
    public DbSet<RandomElement> RandomElements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 配置角色模板
        modelBuilder.Entity<CharacterTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Occupation).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsDefault).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
        });

        // 配置玩家角色
        modelBuilder.Entity<PlayerCharacter>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status).HasDefaultValue("正常");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("datetime('now')");
        });

        // 配置NPC
        modelBuilder.Entity<NonPlayerCharacter>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Type).HasDefaultValue("中立");
            entity.Property(e => e.Status).HasDefaultValue("活著");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("datetime('now')");

            // NPC 與場景的關係
            entity.HasOne(e => e.Scene)
                .WithMany(s => s.NPCs)
                .HasForeignKey(e => e.SceneId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // 配置技能
        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
        });

        // 配置角色技能關聯
        modelBuilder.Entity<CharacterSkill>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");

            entity.HasOne(cs => cs.PlayerCharacter)
                .WithMany(pc => pc.Skills)
                .HasForeignKey(cs => cs.PlayerCharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(cs => cs.Skill)
                .WithMany()
                .HasForeignKey(cs => cs.SkillId)
                .OnDelete(DeleteBehavior.Cascade);

            // 確保同一角色不會有重複的技能
            entity.HasIndex(e => new { e.PlayerCharacterId, e.SkillId }).IsUnique();
        });

        // 配置NPC技能關聯
        modelBuilder.Entity<NpcSkill>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");

            entity.HasOne(ns => ns.NonPlayerCharacter)
                .WithMany(npc => npc.Skills)
                .HasForeignKey(ns => ns.NonPlayerCharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ns => ns.Skill)
                .WithMany()
                .HasForeignKey(ns => ns.SkillId)
                .OnDelete(DeleteBehavior.Cascade);

            // 確保同一NPC不會有重複的技能
            entity.HasIndex(e => new { e.NonPlayerCharacterId, e.SkillId }).IsUnique();
        });

        // 配置物品
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Weight).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Value).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
        });

        // 配置角色物品關聯
        modelBuilder.Entity<CharacterItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Condition).HasDefaultValue("完好");
            entity.Property(e => e.AcquiredAt).HasDefaultValueSql("datetime('now')");

            entity.HasOne(ci => ci.PlayerCharacter)
                .WithMany(pc => pc.Items)
                .HasForeignKey(ci => ci.PlayerCharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ci => ci.Item)
                .WithMany(i => i.CharacterItems)
                .HasForeignKey(ci => ci.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // 配置場景
        modelBuilder.Entity<Scene>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.LightingCondition).HasDefaultValue("正常");
            entity.Property(e => e.Temperature).HasDefaultValue("正常");
            entity.Property(e => e.Status).HasDefaultValue("正常");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("datetime('now')");

            // 場景與劇本的關係
            entity.HasOne(s => s.Scenario)
                .WithMany(sc => sc.Scenes)
                .HasForeignKey(s => s.ScenarioId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // 配置場景物品關聯
        modelBuilder.Entity<SceneItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IsDiscovered).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");

            entity.HasOne(si => si.Scene)
                .WithMany(s => s.Items)
                .HasForeignKey(si => si.SceneId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(si => si.Item)
                .WithMany(i => i.SceneItems)
                .HasForeignKey(si => si.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // 配置劇本
        modelBuilder.Entity<Scenario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).HasDefaultValue("草稿");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("datetime('now')");
        });

        // 配置遊戲會話
        modelBuilder.Entity<GameSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).HasDefaultValue("準備中");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("datetime('now')");

            entity.HasOne(gs => gs.Scenario)
                .WithMany(s => s.GameSessions)
                .HasForeignKey(gs => gs.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // 配置會話角色關聯
        modelBuilder.Entity<SessionCharacter>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.JoinedAt).HasDefaultValueSql("datetime('now')");

            entity.HasOne(sc => sc.GameSession)
                .WithMany(gs => gs.SessionCharacters)
                .HasForeignKey(sc => sc.GameSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(sc => sc.PlayerCharacter)
                .WithMany()
                .HasForeignKey(sc => sc.PlayerCharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            // 確保同一會話中角色不重複
            entity.HasIndex(e => new { e.GameSessionId, e.PlayerCharacterId }).IsUnique();
        });

        // 配置檢定記錄
        modelBuilder.Entity<RollRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RolledAt).HasDefaultValueSql("datetime('now')");

            entity.HasOne(rr => rr.GameSession)
                .WithMany()
                .HasForeignKey(rr => rr.GameSessionId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(rr => rr.PlayerCharacter)
                .WithMany()
                .HasForeignKey(rr => rr.PlayerCharacterId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // 配置SAN值記錄
        modelBuilder.Entity<SanityRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RecordedAt).HasDefaultValueSql("datetime('now')");

            entity.HasOne(sr => sr.GameSession)
                .WithMany()
                .HasForeignKey(sr => sr.GameSessionId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(sr => sr.PlayerCharacter)
                .WithMany()
                .HasForeignKey(sr => sr.PlayerCharacterId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // 配置遊戲記錄
        modelBuilder.Entity<GameLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RecordedAt).HasDefaultValueSql("datetime('now')");

            entity.HasOne(gl => gl.GameSession)
                .WithMany(gs => gs.GameLogs)
                .HasForeignKey(gl => gl.GameSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(gl => gl.PlayerCharacter)
                .WithMany()
                .HasForeignKey(gl => gl.PlayerCharacterId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(gl => gl.NonPlayerCharacter)
                .WithMany()
                .HasForeignKey(gl => gl.NonPlayerCharacterId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(gl => gl.Scene)
                .WithMany()
                .HasForeignKey(gl => gl.SceneId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // 配置角色記錄
        modelBuilder.Entity<CharacterLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RecordedAt).HasDefaultValueSql("datetime('now')");

            entity.HasOne(cl => cl.PlayerCharacter)
                .WithMany(pc => pc.Logs)
                .HasForeignKey(cl => cl.PlayerCharacterId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}