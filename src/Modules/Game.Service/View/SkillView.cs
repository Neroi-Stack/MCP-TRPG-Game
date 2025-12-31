using Game.Service.Data.Models;

namespace Game.Service.View
{
	public class SkillView
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Category { get; set; } = string.Empty;
		public int BaseSuccessRate { get; set; }
		public string Description { get; set; } = string.Empty;
		public int? ParentSkillId { get; set; }
		public bool IsBasic { get; set; }
		public bool IsActive { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public static implicit operator SkillView?(Skill? skill)
		{
			if (skill == null) return null;
			return new SkillView
			{
				Id = skill.Id,
				Name = skill.Name,
				Category = skill.Category,
				BaseSuccessRate = skill.BaseSuccessRate,
				Description = skill.Description,
				ParentSkillId = skill.ParentSkillId,
				IsBasic = skill.IsBasic,
				IsActive = skill.IsActive,
				DisplayOrder = skill.DisplayOrder,
				CreatedAt = skill.CreatedAt,
				UpdatedAt = skill.UpdatedAt
			};
		}
	}
}