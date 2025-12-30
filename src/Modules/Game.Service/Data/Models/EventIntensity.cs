namespace Game.Service.Data.Models
{
	public class EventIntensity
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int Level { get; set; }
		public string Description { get; set; } = string.Empty;

		public ICollection<RandomEvent> RandomEvents { get; set; } = new List<RandomEvent>();
	}
}
