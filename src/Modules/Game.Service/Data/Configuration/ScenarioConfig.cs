using Game.Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Service.Data.Configuration
{
    public class ScenarioConfig : IEntityTypeConfiguration<Scenario>
    {
        public void Configure(EntityTypeBuilder<Scenario> builder)
        {
            builder.ToTable("Scenario");
            builder.HasKey(s => s.Id);
			builder.Property(s => s.Id).HasColumnName("Id");
            builder.Property(s => s.Name).HasColumnName("Name").IsRequired();
            builder.Property(s => s.Description).HasColumnName("Description").IsRequired();
            builder.Property(s => s.Background).HasColumnName("Background").IsRequired();
            builder.Property(s => s.OpeningNarrative).HasColumnName("OpeningNarrative").IsRequired();
            builder.Property(s => s.RecommendedPlayerCount).HasColumnName("RecommendedPlayerCount").IsRequired();
            builder.Property(s => s.EstimatedDuration).HasColumnName("EstimatedDuration").IsRequired();
            builder.Property(s => s.DifficultyLevel).HasColumnName("DifficultyLevel").IsRequired();
            builder.Property(s => s.Tags).HasColumnName("Tags").IsRequired();
            builder.Property(s => s.MainPlotPoints).HasColumnName("MainPlotPoints").IsRequired();
            builder.Property(s => s.TruthAndEndings).HasColumnName("TruthAndEndings").IsRequired();
            builder.Property(s => s.KeeperNotes).HasColumnName("KeeperNotes").IsRequired();
            builder.Property(s => s.DefaultRollsAndSanity).HasColumnName("DefaultRollsAndSanity").IsRequired();
            builder.Property(s => s.IsActive).HasColumnName("IsActive").IsRequired();
            builder.Property(s => s.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(s => s.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();

            builder.HasMany(s => s.Scenes)
                .WithOne(sc => sc.Scenario)
                .HasForeignKey(sc => sc.ScenarioId);

            builder.HasMany(s => s.ScenarioCharacters)
                .WithOne(sc => sc.Scenario)
                .HasForeignKey(sc => sc.ScenarioId);
        }
    }
}
