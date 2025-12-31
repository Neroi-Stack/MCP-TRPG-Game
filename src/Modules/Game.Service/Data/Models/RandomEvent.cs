namespace Game.Service.Data.Models
{
	public class RandomEvent
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int EventIntensityId { get; set; }
		public int? ScenarioId { get; set; }
		public int? SceneId { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int? CheckRequirementId { get; set; }

		public EventIntensity? EventIntensity { get; set; }
		public ICollection<RandomEventElement> RandomEventElements { get; set; } = new List<RandomEventElement>();
	}
}
