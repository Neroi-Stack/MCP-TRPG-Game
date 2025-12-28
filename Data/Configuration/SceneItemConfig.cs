using MCPTRPGGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Services.UserServices.Configuration
{
    public class SceneItemConfig : IEntityTypeConfiguration<SceneItem>
    {
        public void Configure(EntityTypeBuilder<SceneItem> builder)
        {
            builder.ToTable("SceneItem");
            builder.HasKey(si => new { si.SceneId, si.ItemId });
			builder.Property(si => si.SceneId).HasColumnName("SceneId");
			builder.Property(si => si.ItemId).HasColumnName("ItemId");

            builder.HasOne(si => si.Scene)
                .WithMany(s => s.SceneItems)
                .HasForeignKey(si => si.SceneId);
            builder.HasOne(si => si.Item)
                .WithMany(i => i.SceneItems)
                .HasForeignKey(si => si.ItemId);
        }
    }
}
