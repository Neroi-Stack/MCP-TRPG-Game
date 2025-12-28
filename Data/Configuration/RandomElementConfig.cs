using MCPTRPGGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Services.UserServices.Configuration
{
    public class RandomElementConfig : IEntityTypeConfiguration<RandomElement>
    {
        public void Configure(EntityTypeBuilder<RandomElement> builder)
        {
            builder.ToTable("RandomElement");
            builder.HasKey(re => re.Id);
			builder.Property(re => re.Id).HasColumnName("Id");
            builder.Property(re => re.Type).HasColumnName("Type").IsRequired();
            builder.Property(re => re.Description).HasColumnName("Description").IsRequired();
            builder.Property(re => re.Weight).HasColumnName("Weight").IsRequired();
            builder.Property(re => re.CultureTag).HasColumnName("CultureTag").IsRequired();
            builder.Property(re => re.GenderRestriction).HasColumnName("GenderRestriction").IsRequired();
            builder.Property(re => re.OccupationTags).HasColumnName("OccupationTags").IsRequired();
            builder.Property(re => re.AgeGroup).HasColumnName("AgeGroup").IsRequired();
            builder.Property(re => re.IsActive).HasColumnName("IsActive").IsRequired();
            builder.Property(re => re.DisplayOrder).HasColumnName("DisplayOrder").IsRequired();
            builder.Property(re => re.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(re => re.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();

            builder.HasMany(re => re.RandomEventElements)
                .WithOne(ree => ree.RandomElement)
                .HasForeignKey(ree => ree.RandomElementId);
        }
    }
}
