namespace Game.Service.Data.Models
{
	public class RandomElement
	{
		public int Id { get; set; }
		public string Type { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int Weight { get; set; }
		public string CultureTag { get; set; } = string.Empty;
		public string GenderRestriction { get; set; } = string.Empty;
		public string OccupationTags { get; set; } = string.Empty;
		public string AgeGroup { get; set; } = string.Empty;
		public bool IsActive { get; set; }
		public int DisplayOrder { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public ICollection<RandomEventElement> RandomEventElements { get; set; } = new List<RandomEventElement>();
	}
}
