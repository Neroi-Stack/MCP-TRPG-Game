namespace Game.Service.Data.Models
{
	public class RandomEventElement
	{
		public int RandomEventId { get; set; }
		public int RandomElementId { get; set; }

		public RandomEvent? RandomEvent { get; set; }
		public RandomElement? RandomElement { get; set; }
	}
}
