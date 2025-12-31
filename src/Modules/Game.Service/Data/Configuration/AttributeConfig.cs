using Game.Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Game.Service.Data.Configuration
{
	public class AttributeConfig : IEntityTypeConfiguration<Attributes>
	{
		public void Configure(EntityTypeBuilder<Attributes> builder)
		{
			builder.ToTable("Attributes");
			builder.HasKey(a => a.Id);
			builder.Property(a => a.Id).HasColumnName("Id");
			builder.Property(a => a.Name).HasColumnName("Name").IsRequired();
			builder.Property(a => a.Description).HasColumnName("Description").IsRequired();

			builder.HasMany(a => a.CharacterAttributes)
				.WithOne(ca => ca.Attribute)
				.HasForeignKey(ca => ca.AttributeId);
		}
	}
}