using MCPTRPGGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Services.UserServices.Configuration
{
    public class SceneRollSuggestionConfig : IEntityTypeConfiguration<SceneRollSuggestion>
    {
        public void Configure(EntityTypeBuilder<SceneRollSuggestion> builder)
        {
            builder.ToTable("SceneRollSuggestion");
            builder.HasKey(srs => srs.Id);
			builder.Property(srs => srs.Id).HasColumnName("Id");
            builder.Property(srs => srs.Content).HasColumnName("Content").IsRequired();
            builder.Property(srs => srs.Difficulty).HasColumnName("Difficulty").IsRequired();
            builder.Property(srs => srs.KeeperNotes).HasColumnName("KeeperNotes").IsRequired();
            builder.Property(srs => srs.IsActive).HasColumnName("IsActive").IsRequired();
            builder.Property(srs => srs.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(srs => srs.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();

            builder.HasMany(srs => srs.SceneRollSuggestionScenes)
                .WithOne(s => s.SceneRollSuggestion)
                .HasForeignKey(s => s.SceneRollSuggestionId);

            builder.HasMany(srs => srs.SceneRollSuggestionSkills)
                .WithOne(srsks => srsks.SceneRollSuggestion)
                .HasForeignKey(srsks => srsks.SceneRollSuggestionId);
        }
    }
}
