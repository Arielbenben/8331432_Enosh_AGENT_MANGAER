namespace Agents_Rest.Model
{
    public enum StatusMission
    {
        Offer,
        Assigned,
        Eliminated
    }
    public class MissionModel
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public AgentModel Agent { get; set; }
        public int TargetId { get; set; }
        public TargetModel Target { get; set; }
        public double TimeLeft { get; set; }
        public DateTime ExecutionTime { get; set; }
        public StatusMission Status { get; set; } = StatusMission.Offer;

    }
}
