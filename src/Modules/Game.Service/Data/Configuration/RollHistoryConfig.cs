using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Game.Service.Data.Models;

namespace Game.Service.Data.Configuration
{
    public class RollHistoryConfig : IEntityTypeConfiguration<RollHistory>
    {
        public void Configure(EntityTypeBuilder<RollHistory> builder)
        {
            builder.ToTable("RollHistory");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).HasColumnName("Id");
            builder.Property(r => r.PlayerCharacterId).HasColumnName("PlayerCharacterId");
            builder.Property(r => r.SkillId).HasColumnName("SkillId");
            builder.Property(r => r.RollType).HasColumnName("RollType");
            builder.Property(r => r.Expression).HasColumnName("Expression");
            builder.Property(r => r.Result).HasColumnName("Result");
            builder.Property(r => r.IsSuccess).HasColumnName("IsSuccess");
            builder.Property(r => r.Timestamp).HasColumnName("Timestamp").IsRequired();
            builder.Property(r => r.Note).HasColumnName("Note");
        }
    }
}
