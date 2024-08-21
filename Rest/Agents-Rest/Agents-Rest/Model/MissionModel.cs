namespace Agents_Rest.Model
{
    public enum StatusMission
    {
        Offer,
        ConnectToMission,
        Done
    }
    public class MissionModel
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public AgentModel Agent { get; set; }
        public int TargetId { get; set; }
        public TargetModel Target { get; set; }
        public DateTime TimeLeft { get; set; }
        public DateTime ExecutionTime { get; set; }
        public StatusMission Status { get; set; } = StatusMission.Offer;

    }
}
