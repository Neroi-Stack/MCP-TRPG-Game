namespace Game.Service.Data.Models
{
    public class RollHistory
    {
        public int Id { get; set; }
        public int? PlayerCharacterId { get; set; }
        public int? SkillId { get; set; }
        public string? RollType { get; set; }
        public string? Expression { get; set; }
	    public int Result { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Note { get; set; }
    }
}
