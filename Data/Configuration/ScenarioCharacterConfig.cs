using MCPTRPGGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Services.UserServices.Configuration
{
    public class ScenarioCharacterConfig : IEntityTypeConfiguration<ScenarioCharacter>
    {
        public void Configure(EntityTypeBuilder<ScenarioCharacter> builder)
        {
            builder.ToTable("ScenarioCharacter");
            builder.HasKey(sc => new { sc.ScenarioId, sc.CharacterId });
			builder.Property(sc => sc.ScenarioId).HasColumnName("ScenarioId");
			builder.Property(sc => sc.CharacterId).HasColumnName("CharacterId");

            builder.HasOne(sc => sc.Scenario)
                .WithMany(s => s.ScenarioCharacters)
                .HasForeignKey(sc => sc.ScenarioId);

            // CharacterId can refer to either a PlayerCharacter or NonPlayerCharacter
            // Keep navigation configuration minimal to avoid ambiguous FK mappings.
        }
    }
}
