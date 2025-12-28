using MCPTRPGGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace backend.Services.UserServices.Configuration
{
	public class NpcReactionConfig : IEntityTypeConfiguration<NpcReaction>
	{
		public void Configure(EntityTypeBuilder<NpcReaction> builder)
		{
			builder.ToTable("NpcReaction");
			builder.HasKey(npr => npr.Id);
			builder.Property(npr => npr.Id).HasColumnName("Id");
			builder.Property(npr => npr.NonPlayerCharacterId).HasColumnName("NonPlayerCharacterId").IsRequired();
			builder.Property(npr => npr.ReactionType).HasColumnName("ReactionType").IsRequired();
			builder.Property(npr => npr.Content).HasColumnName("Content").IsRequired();
			builder.Property(npr => npr.Trigger).HasColumnName("Trigger").IsRequired();
			builder.Property(npr => npr.Probability).HasColumnName("Probability").IsRequired();
			builder.Property(npr => npr.IsActive).HasColumnName("IsActive").IsRequired();
			builder.Property(npr => npr.CreatedAt).HasColumnName("CreatedAt").IsRequired();
			builder.Property(npr => npr.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();
			builder.Property(npr => npr.DisplayOrder).HasColumnName("DisplayOrder").IsRequired();
			builder.Property(npr => npr.CheckRequirementId).HasColumnName("CheckRequirementId").IsRequired(false);
			builder.HasMany(npr => npr.ActionSuggestionNpcReactions)
				.WithOne(asnr => asnr.NpcReaction)
				.HasForeignKey(asnr => asnr.NpcReactionId);
			builder.HasOne(npr => npr.NonPlayerCharacter)
				.WithMany(npc => npc.NpcReactions)
				.HasForeignKey(npr => npr.NonPlayerCharacterId);
		}
	}
}
