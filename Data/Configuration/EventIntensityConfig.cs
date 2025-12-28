using MCPTRPGGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace backend.Services.UserServices.Configuration
{
	public class EventIntensityConfig : IEntityTypeConfiguration<EventIntensity>
	{
		public void Configure(EntityTypeBuilder<EventIntensity> builder)
		{
			builder.ToTable("EventIntensity");
			builder.HasKey(ei => ei.Id);
			builder.Property(ei => ei.Id).HasColumnName("Id");
			builder.Property(ei => ei.Name).HasColumnName("Name").IsRequired();
			builder.Property(ei => ei.Level).HasColumnName("Level").IsRequired();
			builder.Property(ei => ei.Description).HasColumnName("Description").IsRequired();
		}
	}
}
