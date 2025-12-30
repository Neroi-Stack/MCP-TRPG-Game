using Game.Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Game.Service.Data.Configuration
{
	public class CharacterSkillConfig : IEntityTypeConfiguration<CharacterSkill>
	{
		public void Configure(EntityTypeBuilder<CharacterSkill> builder)
		{
			builder.ToTable("CharacterSkill");
			builder.HasKey(cs => new { cs.CharacterId, cs.SkillId });
			builder.Property(cs => cs.CharacterId).HasColumnName("CharacterId");
			builder.Property(cs => cs.SkillId).HasColumnName("SkillId");
			builder.Property(cs => cs.Proficiency).HasColumnName("Proficiency").IsRequired();

			// Prevent EF from creating an automatic relationship to NonPlayerCharacter
			builder.Ignore(cs => cs.NonPlayerCharacter);

			builder.HasOne(cs => cs.PlayerCharacter)
				.WithMany(pc => pc.CharacterSkills)
				.HasForeignKey(cs => cs.CharacterId);
			builder.HasOne(cs => cs.Skill)
				.WithMany(s => s.CharacterSkills)
				.HasForeignKey(cs => cs.SkillId);
		}
	}
}