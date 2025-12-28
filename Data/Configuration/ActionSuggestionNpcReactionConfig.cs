using MCPTRPGGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace backend.Services.UserServices.Configuration
{
	public class ActionSuggestionNpcReactionConfig : IEntityTypeConfiguration<ActionSuggestionNpcReaction>
	{
		public void Configure(EntityTypeBuilder<ActionSuggestionNpcReaction> builder)
		{
			builder.ToTable("ActionSuggestionNpcReaction");
			builder.HasKey(a => new { a.ActionSuggestionId, a.NpcReactionId });
			builder.Property(asnr => asnr.ActionSuggestionId).HasColumnName("ActionSuggestionId");
			builder.Property(asnr => asnr.NpcReactionId).HasColumnName("NpcReactionId");

			builder.HasOne(asnr => asnr.ActionSuggestion)
				.WithMany(a => a.ActionSuggestionNpcReactions)
				.HasForeignKey(asnr => asnr.ActionSuggestionId);
			builder.HasOne(asnr => asnr.NpcReaction)
				.WithMany(nr => nr.ActionSuggestionNpcReactions)
				.HasForeignKey(asnr => asnr.NpcReactionId);
		}
	}
}