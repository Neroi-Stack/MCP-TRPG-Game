using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Game.Service.Data.Models;

namespace Game.Service.Data.Configuration
{
    public class ProfessionConfig : IEntityTypeConfiguration<Profession>
    {
        public void Configure(EntityTypeBuilder<Profession> builder)
        {
            builder.ToTable("Profession");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id");
            builder.Property(p => p.Name).HasColumnName("Name").IsRequired().HasMaxLength(64);
            builder.Property(p => p.Description).HasColumnName("Description").HasMaxLength(256);
            builder.Property(p => p.SkillPointFormula).HasColumnName("SkillPointFormula").IsRequired();

                 builder.HasMany(p => p.ProfessionSkills)
                     .WithOne(ps => ps.Profession)
                     .HasForeignKey(ps => ps.ProfessionId);
        }
    }
}
