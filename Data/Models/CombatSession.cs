namespace MCPTRPGGame.Data.Models
{
    public class CombatSession
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Participants { get; set; }
        public string? Status { get; set; }

        public ICollection<CombatAction> Actions { get; set; } = new List<CombatAction>();
    }
}
