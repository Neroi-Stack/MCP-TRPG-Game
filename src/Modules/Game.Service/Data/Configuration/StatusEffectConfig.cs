using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Game.Service.Data.Models;

namespace Game.Service.Data.Configuration
{
    public class StatusEffectConfig : IEntityTypeConfiguration<StatusEffect>
    {
        public void Configure(EntityTypeBuilder<StatusEffect> builder)
        {
            builder.ToTable("StatusEffect");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("Id");
            builder.Property(s => s.Name).HasColumnName("Name").IsRequired().HasMaxLength(64);
            builder.Property(s => s.Description).HasColumnName("Description").HasMaxLength(256);
            builder.Property(s => s.DurationRounds).HasColumnName("DurationRounds");
        }
    }
}
