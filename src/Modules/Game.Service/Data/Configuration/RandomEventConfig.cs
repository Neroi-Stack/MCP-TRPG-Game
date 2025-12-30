using Game.Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Service.Data.Configuration
{
    public class RandomEventConfig : IEntityTypeConfiguration<RandomEvent>
    {
        public void Configure(EntityTypeBuilder<RandomEvent> builder)
        {
            builder.ToTable("RandomEvent");
            builder.HasKey(re => re.Id);
			builder.Property(re => re.Id).HasColumnName("Id");
            builder.Property(re => re.Name).HasColumnName("Name").IsRequired();
            builder.Property(re => re.Description).HasColumnName("Description").IsRequired();
            builder.Property(re => re.EventIntensityId).HasColumnName("EventIntensityId").IsRequired();
            builder.Property(re => re.ScenarioId).HasColumnName("ScenarioId").IsRequired(false);
            builder.Property(re => re.SceneId).HasColumnName("SceneId").IsRequired(false);
            builder.Property(re => re.IsActive).HasColumnName("IsActive").IsRequired();
            builder.Property(re => re.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(re => re.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();
            builder.Property(re => re.CheckRequirementId).HasColumnName("CheckRequirementId").IsRequired(false);

            builder.HasOne(re => re.EventIntensity)
                .WithMany(ei => ei.RandomEvents)
                .HasForeignKey(re => re.EventIntensityId);

            builder.HasMany(re => re.RandomEventElements)
                .WithOne(ree => ree.RandomEvent)
                .HasForeignKey(ree => ree.RandomEventId);
        }
    }
}
