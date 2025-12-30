using Game.Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Game.Service.Data.Configuration
{
	public class PlayerCharacterConfig : IEntityTypeConfiguration<PlayerCharacter>
	{
		public void Configure(EntityTypeBuilder<PlayerCharacter> builder)
		{
			builder.ToTable("PlayerCharacter");
			builder.HasKey(pc => pc.Id);
			builder.Property(pc => pc.Id).HasColumnName("Id");
			builder.Property(pc => pc.Name).HasColumnName("Name").IsRequired();
			builder.Property(pc => pc.Gender).HasColumnName("Gender").IsRequired();
			builder.Property(pc => pc.Age).HasColumnName("Age").IsRequired();
			builder.Property(pc => pc.PhysicalDesc).HasColumnName("PhysicalDesc").IsRequired();
			builder.Property(pc => pc.Biography).HasColumnName("Biography").IsRequired();
			builder.Property(pc => pc.StatusEffects).HasColumnName("StatusEffects").IsRequired();
			builder.Property(pc => pc.Notes).HasColumnName("Notes").IsRequired();
			builder.Property(pc => pc.CharacterTemplateId).HasColumnName("CharacterTemplateId").IsRequired(false);
			builder.Property(pc => pc.IsDead).HasColumnName("IsDead").IsRequired();
			builder.Property(pc => pc.LastKnownSceneId).HasColumnName("LastKnownSceneId").IsRequired(false);
			builder.Property(pc => pc.IsTemplate).HasColumnName("IsTemplate").IsRequired();
			builder.Property(pc => pc.IsActive).HasColumnName("IsActive").IsRequired();
			builder.Property(pc => pc.CreatedAt).HasColumnName("CreatedAt").IsRequired();
			builder.Property(pc => pc.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();
			builder.HasMany(pc => pc.CharacterSkills)
				.WithOne(cs => cs.PlayerCharacter)
				.HasForeignKey(cs => cs.CharacterId);
			builder.HasMany(pc => pc.CharacterItems)
				.WithOne(ci => ci.PlayerCharacter)
				.HasForeignKey(ci => ci.CharacterId);
			builder.HasMany(pc => pc.CharacterAttributes)
				.WithOne(ca => ca.PlayerCharacter)
				.HasForeignKey(ca => ca.CharacterId);
			builder.HasMany(pc => pc.CharacterActionSuggestions)
				.WithOne(cas => cas.PlayerCharacter)
				.HasForeignKey(cas => cas.CharacterId);
		}
	}
}
