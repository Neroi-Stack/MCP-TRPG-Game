using MCPTRPGGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Services.UserServices.Configuration
{
    public class SceneRollSuggestionSkillConfig : IEntityTypeConfiguration<SceneRollSuggestionSkill>
    {
        public void Configure(EntityTypeBuilder<SceneRollSuggestionSkill> builder)
        {
            builder.ToTable("SceneRollSuggestionSkill");
            builder.HasKey(s => new { s.SceneRollSuggestionId, s.SkillId });
			builder.Property(s => s.SceneRollSuggestionId).HasColumnName("SceneRollSuggestionId");
			builder.Property(s => s.SkillId).HasColumnName("SkillId");

            builder.HasOne(s => s.SceneRollSuggestion)
                .WithMany(srs => srs.SceneRollSuggestionSkills)
                .HasForeignKey(s => s.SceneRollSuggestionId);
            builder.HasOne(s => s.Skill)
                .WithMany(sk => sk.SceneRollSuggestionSkills)
                .HasForeignKey(s => s.SkillId);
        }
    }
}
