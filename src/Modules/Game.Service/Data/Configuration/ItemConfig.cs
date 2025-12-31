using Game.Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Game.Service.Data.Configuration
{
	public class ItemConfig : IEntityTypeConfiguration<Item>
	{
		public void Configure(EntityTypeBuilder<Item> builder)
		{
			builder.ToTable("Item");
			builder.HasKey(i => i.Id);
			builder.Property(i => i.Id).HasColumnName("Id");
			builder.Property(i => i.Name).HasColumnName("Name").IsRequired();
			builder.Property(i => i.Category).HasColumnName("Category").IsRequired();
			builder.Property(i => i.Description).HasColumnName("Description").IsRequired();
			builder.Property(i => i.Stats).HasColumnName("Stats").IsRequired();
			builder.Property(i => i.OwnerNotes).HasColumnName("OwnerNotes").IsRequired();
			builder.Property(i => i.Weight).HasColumnName("Weight").IsRequired();
			builder.Property(i => i.IsConsumable).HasColumnName("IsConsumable").IsRequired();
			builder.Property(i => i.IsCursed).HasColumnName("IsCursed").IsRequired();
			builder.Property(i => i.IsActive).HasColumnName("IsActive").IsRequired();
			builder.Property(i => i.DisplayOrder).HasColumnName("DisplayOrder").IsRequired();
			builder.Property(i => i.CreatedAt).HasColumnName("CreatedAt").IsRequired();
			builder.Property(i => i.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();

			builder.HasMany(i => i.CharacterItems)
				.WithOne(ci => ci.Item)
				.HasForeignKey(ci => ci.ItemId);
			builder.HasMany(i => i.SceneItems)
				.WithOne(si => si.Item)
				.HasForeignKey(si => si.ItemId);
		}
	}
}
