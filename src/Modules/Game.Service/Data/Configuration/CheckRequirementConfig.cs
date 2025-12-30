using Game.Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Game.Service.Data.Configuration
{
	public class CheckRequirementConfig : IEntityTypeConfiguration<CheckRequirement	>
	{
		public void Configure(EntityTypeBuilder<CheckRequirement> builder)
		{
			builder.ToTable("CheckRequirement");
			builder.HasKey(cr => cr.Id);
			builder.Property(cr => cr.Id).HasColumnName("Id");
			builder.Property(cr => cr.SkillId).HasColumnName("SkillId").IsRequired(false);
			builder.Property(cr => cr.AttributeId).HasColumnName("AttributeId").IsRequired(false);
			builder.Property(cr => cr.DiceExpression).HasColumnName("DiceExpression").IsRequired();
			builder.Property(cr => cr.Difficulty).HasColumnName("Difficulty").IsRequired();
			builder.Property(cr => cr.Description).HasColumnName("Description").IsRequired();
			builder.Property(cr => cr.KeeperNotes).HasColumnName("KeeperNotes").IsRequired();
			builder.Property(cr => cr.IsActive).HasColumnName("IsActive").IsRequired();
			builder.Property(cr => cr.DisplayOrder).HasColumnName("DisplayOrder").IsRequired();
		}
	}
}
