using Game.Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Service.Data.Configuration
{
    public class RandomEventElementConfig : IEntityTypeConfiguration<RandomEventElement>
    {
        public void Configure(EntityTypeBuilder<RandomEventElement> builder)
        {
            builder.ToTable("RandomEventElement");
            builder.HasKey(ree => new { ree.RandomEventId, ree.RandomElementId });
			builder.Property(ree => ree.RandomEventId).HasColumnName("RandomEventId");
			builder.Property(ree => ree.RandomElementId).HasColumnName("RandomElementId");

            builder.HasOne(ree => ree.RandomEvent)
                .WithMany(re => re.RandomEventElements)
                .HasForeignKey(ree => ree.RandomEventId);
            builder.HasOne(ree => ree.RandomElement)
                .WithMany(re => re.RandomEventElements)
                .HasForeignKey(ree => ree.RandomElementId);
        }
    }
}
