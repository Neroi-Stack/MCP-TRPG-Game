using MCPTRPGGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace backend.Services.UserServices.Configuration
{
	public class ActionSuggestionConfig : IEntityTypeConfiguration<ActionSuggestion>
	{
		public void Configure(EntityTypeBuilder<ActionSuggestion> builder)
		{
			builder.ToTable("ActionSuggestion");
			builder.HasKey(a => a.Id);
			builder.Property(a => a.Id).HasColumnName("Id");
			builder.Property(a => a.SuggestionType).HasColumnName("SuggestionType").IsRequired();
			builder.Property(a => a.Content).HasColumnName("Content").IsRequired();
			builder.Property(a => a.Probability).HasColumnName("Probability").IsRequired();
			builder.Property(a => a.KeeperNotes).HasColumnName("KeeperNotes").IsRequired();
			builder.Property(a => a.IsActive).HasColumnName("IsActive").IsRequired();
			builder.Property(a => a.DisplayOrder).HasColumnName("DisplayOrder").IsRequired();
			builder.Property(a => a.CreatedAt).HasColumnName("CreatedAt").IsRequired();
			builder.Property(a => a.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();
			builder.Property(a => a.CheckRequirementId).HasColumnName("CheckRequirementId").IsRequired(false);

			builder.HasMany(a => a.SceneActionSuggestions)
				.WithOne(sas => sas.ActionSuggestion)
				.HasForeignKey(sas => sas.ActionSuggestionId);
			builder.HasMany(a => a.CharacterActionSuggestions)
				.WithOne(cas => cas.ActionSuggestion)
				.HasForeignKey(cas => cas.ActionSuggestionId);
			builder.HasMany(a => a.ActionSuggestionNpcReactions)
				.WithOne(asnr => asnr.ActionSuggestion)
				.HasForeignKey(asnr => asnr.ActionSuggestionId);

			builder.HasMany(a => a.CharacterActionSuggestions)
				.WithOne(cas => cas.ActionSuggestion)
				.HasForeignKey(cas => cas.ActionSuggestionId);

			builder.HasMany(a => a.ActionSuggestionNpcReactions)
				.WithOne(asnr => asnr.ActionSuggestion)
				.HasForeignKey(asnr => asnr.ActionSuggestionId);
		}
	}
}