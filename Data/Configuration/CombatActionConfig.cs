using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MCPTRPGGame.Data.Models;

namespace MCPTRPGGame.Data.Configuration
{
    public class CombatActionConfig : IEntityTypeConfiguration<CombatAction>
    {
        public void Configure(EntityTypeBuilder<CombatAction> builder)
        {
            builder.ToTable("CombatAction");
            builder.HasKey(ca => ca.Id);
            builder.Property(ca => ca.Id).HasColumnName("Id");
            builder.Property(ca => ca.CombatSessionId).HasColumnName("CombatSessionId");
            builder.Property(ca => ca.ActorId).HasColumnName("ActorId");
            builder.Property(ca => ca.ActionType).HasColumnName("ActionType");
            builder.Property(ca => ca.Target).HasColumnName("Target");
            builder.Property(ca => ca.Detail).HasColumnName("Detail");
            builder.Property(ca => ca.Damage).HasColumnName("Damage");
            builder.Property(ca => ca.Timestamp).HasColumnName("Timestamp").IsRequired();
        }
    }
}
