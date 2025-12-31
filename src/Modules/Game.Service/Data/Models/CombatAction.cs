namespace Game.Service.Data.Models
{
    public class CombatAction
    {
        public int Id { get; set; }
        public int CombatSessionId { get; set; }
        public int ActorId { get; set; }
        public string? ActionType { get; set; }
        public string? Target { get; set; }
        public string? Detail { get; set; }
        public int? Damage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
