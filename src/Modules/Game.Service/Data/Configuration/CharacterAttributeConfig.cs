using Game.Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Game.Service.Data.Configuration
{
	public class CharacterAttributeConfig : IEntityTypeConfiguration<CharacterAttribute>
	{
		public void Configure(EntityTypeBuilder<CharacterAttribute> builder)
		{
			builder.ToTable("CharacterAttribute");
			builder.HasKey(ca => new { ca.CharacterId, ca.AttributeId });
			builder.Property(ca => ca.CharacterId).HasColumnName("CharacterId");
			builder.Property(ca => ca.AttributeId).HasColumnName("AttributeId");
			builder.Property(ca => ca.MaxValue).HasColumnName("MaxValue").IsRequired();
			builder.Property(ca => ca.CurrentValue).HasColumnName("CurrentValue").IsRequired();

			// Prevent EF from creating an automatic relationship to NonPlayerCharacter
			builder.Ignore(ca => ca.NonPlayerCharacter);
	
			builder.HasOne(ca => ca.PlayerCharacter)
				.WithMany(pc => pc.CharacterAttributes)
				.HasForeignKey(ca => ca.CharacterId);
			builder.HasOne(ca => ca.Attribute)
				.WithMany(a => a.CharacterAttributes)
				.HasForeignKey(ca => ca.AttributeId);
		}
	}
}