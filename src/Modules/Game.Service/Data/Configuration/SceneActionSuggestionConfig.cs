using Game.Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Service.Data.Configuration
{
    public class SceneActionSuggestionConfig : IEntityTypeConfiguration<SceneActionSuggestion>
    {
        public void Configure(EntityTypeBuilder<SceneActionSuggestion> builder)
        {
            builder.ToTable("SceneActionSuggestion");
            builder.HasKey(sas => new { sas.SceneId, sas.ActionSuggestionId });
			builder.Property(sas => sas.SceneId).HasColumnName("SceneId");
			builder.Property(sas => sas.ActionSuggestionId).HasColumnName("ActionSuggestionId");
			
            builder.HasOne(sas => sas.Scene)
                .WithMany(s => s.SceneActionSuggestions)
                .HasForeignKey(sas => sas.SceneId);
            builder.HasOne(sas => sas.ActionSuggestion)
                .WithMany(a => a.SceneActionSuggestions)
                .HasForeignKey(sas => sas.ActionSuggestionId);
        }
    }
}
