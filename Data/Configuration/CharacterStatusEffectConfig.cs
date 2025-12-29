using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MCPTRPGGame.Data.Models;

namespace MCPTRPGGame.Data.Configuration
{
    public class CharacterStatusEffectConfig : IEntityTypeConfiguration<CharacterStatusEffect>
    {
        public void Configure(EntityTypeBuilder<CharacterStatusEffect> builder)
        {
            builder.ToTable("CharacterStatusEffect");
            builder.HasKey(cse => new { cse.CharacterId, cse.StatusEffectId });
            builder.Property(cse => cse.CharacterId).HasColumnName("CharacterId");
            builder.Property(cse => cse.StatusEffectId).HasColumnName("StatusEffectId");
            builder.Property(cse => cse.AppliedAt).HasColumnName("AppliedAt").IsRequired();
            builder.Property(cse => cse.RemainingRounds).HasColumnName("RemainingRounds").IsRequired();
        }
    }
}
