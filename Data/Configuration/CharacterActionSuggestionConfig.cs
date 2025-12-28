using MCPTRPGGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace backend.Services.UserServices.Configuration
{
	public class CharacterActionSuggestionConfig : IEntityTypeConfiguration<CharacterActionSuggestion>
	{
		public void Configure(EntityTypeBuilder<CharacterActionSuggestion> builder)
		{
			builder.ToTable("CharacterActionSuggestion");
			builder.HasKey(cas => new { cas.CharacterId, cas.ActionSuggestionId });
			builder.Property(cas => cas.CharacterId).HasColumnName("CharacterId");
			builder.Property(cas => cas.ActionSuggestionId).HasColumnName("ActionSuggestionId");

			builder.HasOne(cas => cas.PlayerCharacter)
				.WithMany(pc => pc.CharacterActionSuggestions)
				.HasForeignKey(cas => cas.CharacterId);
			builder.HasOne(cas => cas.ActionSuggestion)
				.WithMany(a => a.CharacterActionSuggestions)
				.HasForeignKey(cas => cas.ActionSuggestionId);
		}
	}
}