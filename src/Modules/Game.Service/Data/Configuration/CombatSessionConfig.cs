using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Game.Service.Data.Models;

namespace Game.Service.Data.Configuration
{
    public class CombatSessionConfig : IEntityTypeConfiguration<CombatSession>
    {
        public void Configure(EntityTypeBuilder<CombatSession> builder)
        {
            builder.ToTable("CombatSession");
            builder.HasKey(cs => cs.Id);
            builder.Property(cs => cs.Id).HasColumnName("Id");
            builder.Property(cs => cs.StartTime).HasColumnName("StartTime").IsRequired();
            builder.Property(cs => cs.EndTime).HasColumnName("EndTime");
            builder.Property(cs => cs.Participants).HasColumnName("Participants");
            builder.Property(cs => cs.Status).HasColumnName("Status");
        }
    }
}
