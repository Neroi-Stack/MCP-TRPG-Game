using Game.Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Service.Data.Configuration
{
    public class SkillConfig : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ToTable("Skill");
            builder.HasKey(s => s.Id);
			builder.Property(s => s.Id).HasColumnName("Id");
            builder.Property(s => s.Name).HasColumnName("Name").IsRequired();
            builder.Property(s => s.Category).HasColumnName("Category").IsRequired();
            builder.Property(s => s.BaseSuccessRate).HasColumnName("BaseSuccessRate").IsRequired();
            builder.Property(s => s.Description).HasColumnName("Description").IsRequired();
            builder.Property(s => s.ParentSkillId).HasColumnName("ParentSkillId").IsRequired(false);
			builder.Property(s => s.IsBasic).HasColumnName("IsBasic").IsRequired();
            builder.Property(s => s.IsActive).HasColumnName("IsActive").IsRequired();
            builder.Property(s => s.DisplayOrder).HasColumnName("DisplayOrder").IsRequired();
            builder.Property(s => s.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(s => s.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();

            builder.HasOne(s => s.ParentSkill)
                .WithMany()
                .HasForeignKey(s => s.ParentSkillId);

            builder.HasMany(s => s.CharacterSkills)
                .WithOne(cs => cs.Skill)
                .HasForeignKey(cs => cs.SkillId);
            builder.HasMany(s => s.SceneRollSuggestionSkills)
                .WithOne(srsks => srsks.Skill)
                .HasForeignKey(srsks => srsks.SkillId);
        }
    }
}
