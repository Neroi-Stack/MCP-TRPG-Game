using Game.Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Game.Service.Data.Configuration
{
	public class CharacterItemConfig : IEntityTypeConfiguration<CharacterItem>
	{
		public void Configure(EntityTypeBuilder<CharacterItem> builder)
		{
			builder.ToTable("CharacterItem");
			builder.HasKey(ci => new { ci.CharacterId, ci.ItemId });
			builder.Property(ci => ci.CharacterId).HasColumnName("CharacterId");
			builder.Property(ci => ci.ItemId).HasColumnName("ItemId");
			builder.Property(ci => ci.Quantity).HasColumnName("Quantity").IsRequired();

			// Prevent EF from creating an automatic relationship to NonPlayerCharacter
			builder.Ignore(ci => ci.NonPlayerCharacter);

			builder.HasOne(ci => ci.PlayerCharacter)
				.WithMany(pc => pc.CharacterItems)
				.HasForeignKey(ci => ci.CharacterId);
			builder.HasOne(ci => ci.Item)
				.WithMany(i => i.CharacterItems)
				.HasForeignKey(ci => ci.ItemId);
		}
	}
}