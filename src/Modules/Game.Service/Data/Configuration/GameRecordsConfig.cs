using Game.Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Game.Service.Data.Configuration
{
	public class GameRecordsConfig : IEntityTypeConfiguration<GameRecords>
	{
		public void Configure(EntityTypeBuilder<GameRecords> builder)
		{
			builder.ToTable("GameRecords");
			builder.HasKey(gr => gr.Id);
			builder.Property(gr => gr.Id).HasColumnName("Id");
			builder.Property(gr => gr.Description).HasColumnName("Description").IsRequired();
			builder.Property(gr => gr.RecordType).HasColumnName("RecordType").IsRequired();
			builder.Property(gr => gr.ActorId).HasColumnName("ActorId").IsRequired(false);
			builder.Property(gr => gr.ActorType).HasColumnName("ActorType").IsRequired();
			builder.Property(gr => gr.SceneId).HasColumnName("SceneId").IsRequired(false);
			builder.Property(gr => gr.ScenarioId).HasColumnName("ScenarioId").IsRequired(false);
			builder.Property(gr => gr.RandomEventId).HasColumnName("RandomEventId").IsRequired(false);
			builder.Property(gr => gr.ActionTime).HasColumnName("ActionTime").IsRequired();
			builder.Property(gr => gr.ResultJson).HasColumnName("ResultJson").IsRequired();
			builder.Property(gr => gr.KeeperNotes).HasColumnName("KeeperNotes").IsRequired();
			builder.Property(gr => gr.CreatedAt).HasColumnName("CreatedAt").IsRequired();
		}
	}
}
