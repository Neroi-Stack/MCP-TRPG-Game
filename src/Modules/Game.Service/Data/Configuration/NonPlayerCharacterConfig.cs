using Game.Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Game.Service.Data.Configuration
{
	public class NonPlayerCharacterConfig : IEntityTypeConfiguration<NonPlayerCharacter>
	{
		public void Configure(EntityTypeBuilder<NonPlayerCharacter> builder)
		{
			builder.ToTable("NonPlayerCharacter");
			builder.HasKey(npc => npc.Id);
			builder.Property(npc => npc.Id).HasColumnName("Id");
			builder.Property(npc => npc.Name).HasColumnName("Name").IsRequired();
			builder.Property(npc => npc.Gender).HasColumnName("Gender").IsRequired();
			builder.Property(npc => npc.Age).HasColumnName("Age").IsRequired();
			builder.Property(npc => npc.PhysicalDesc).HasColumnName("PhysicalDesc").IsRequired();
			builder.Property(npc => npc.Background).HasColumnName("Background").IsRequired();
			builder.Property(npc => npc.Role).HasColumnName("Role").IsRequired();
			builder.Property(npc => npc.StatusEffects).HasColumnName("StatusEffects").IsRequired();
			builder.Property(npc => npc.Notes).HasColumnName("Notes").IsRequired();
			builder.Property(npc => npc.CharacterTemplateId).HasColumnName("CharacterTemplateId").IsRequired(false);
			builder.Property(npc => npc.IsHostile).HasColumnName("IsHostile").IsRequired();
			builder.Property(npc => npc.LastKnownSceneId).HasColumnName("LastKnownSceneId").IsRequired(false);
			builder.Property(npc => npc.IsActive).HasColumnName("IsActive").IsRequired();
			builder.Property(npc => npc.CreatedAt).HasColumnName("CreatedAt").IsRequired();
			builder.Property(npc => npc.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();
			// Character join relationships are configured on the join entity side
			builder.HasMany(npc => npc.NpcReactions)
				.WithOne(npr => npr.NonPlayerCharacter)
				.HasForeignKey(npr => npr.NonPlayerCharacterId);
		}
	}
}
