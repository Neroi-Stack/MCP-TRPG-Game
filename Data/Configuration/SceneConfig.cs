using MCPTRPGGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Services.UserServices.Configuration
{
    public class SceneConfig : IEntityTypeConfiguration<Scene>
    {
        public void Configure(EntityTypeBuilder<Scene> builder)
        {
            builder.ToTable("Scene");
            builder.HasKey(s => s.Id);
			builder.Property(s => s.Id).HasColumnName("Id");
            builder.Property(s => s.ScenarioId).HasColumnName("ScenarioId").IsRequired();
            builder.Property(s => s.Name).HasColumnName("Name").IsRequired();
            builder.Property(s => s.Description).HasColumnName("Description").IsRequired();
            builder.Property(s => s.Background).HasColumnName("Background").IsRequired();
            builder.Property(s => s.OrderInScenario).HasColumnName("OrderInScenario").IsRequired();
            builder.Property(s => s.OpeningNarrative).HasColumnName("OpeningNarrative").IsRequired();
            builder.Property(s => s.KeeperNotes).HasColumnName("KeeperNotes").IsRequired();
            builder.Property(s => s.IsActive).HasColumnName("IsActive").IsRequired();
            builder.Property(s => s.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(s => s.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();
            builder.Property(s => s.CheckRequirementId).HasColumnName("CheckRequirementId").IsRequired(false);

            builder.HasOne(s => s.Scenario)
                .WithMany(sc => sc.Scenes)
                .HasForeignKey(s => s.ScenarioId);

            builder.HasMany(s => s.SceneItems)
                .WithOne(si => si.Scene)
                .HasForeignKey(si => si.SceneId);

            builder.HasMany(s => s.SceneActionSuggestions)
                .WithOne(sas => sas.Scene)
                .HasForeignKey(sas => sas.SceneId);

            builder.HasMany(s => s.SceneRollSuggestionScenes)
                .WithOne(srs => srs.Scene)
                .HasForeignKey(srs => srs.SceneId);
        }
    }
}
