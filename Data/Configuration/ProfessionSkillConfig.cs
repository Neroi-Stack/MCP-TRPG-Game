using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MCPTRPGGame.Data.Models;

namespace MCPTRPGGame.Data.Configuration
{
    public class ProfessionSkillConfig : IEntityTypeConfiguration<ProfessionSkill>
    {
        public void Configure(EntityTypeBuilder<ProfessionSkill> builder)
        {
            builder.ToTable("ProfessionSkill");
            builder.HasKey(ps => new { ps.ProfessionId, ps.SkillId });
            builder.Property(ps => ps.ProfessionId).HasColumnName("ProfessionId");
            builder.Property(ps => ps.SkillId).HasColumnName("SkillId");
        }
    }
}
