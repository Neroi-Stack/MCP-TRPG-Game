using Game.Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Service.Data.Configuration
{
    public class SceneRollSuggestionSceneConfig : IEntityTypeConfiguration<SceneRollSuggestionScene>
    {
        public void Configure(EntityTypeBuilder<SceneRollSuggestionScene> builder)
        {
            builder.ToTable("SceneRollSuggestionScene");
            builder.HasKey(s => new { s.SceneRollSuggestionId, s.SceneId });
			builder.Property(s => s.SceneRollSuggestionId).HasColumnName("SceneRollSuggestionId");
			builder.Property(s => s.SceneId).HasColumnName("SceneId");

            builder.HasOne(s => s.SceneRollSuggestion)
                .WithMany(srs => srs.SceneRollSuggestionScenes)
                .HasForeignKey(s => s.SceneRollSuggestionId);
            builder.HasOne(s => s.Scene)
                .WithMany(sc => sc.SceneRollSuggestionScenes)
                .HasForeignKey(s => s.SceneId);
        }
    }
}
