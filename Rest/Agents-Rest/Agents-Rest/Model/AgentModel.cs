namespace Agents_Rest.Model
{
    public enum StatusAgent
    {
        Dormant,
        Active
    }

    public class AgentModel
    {
        public int Id { get; set; }
        public string NickName {  get; set; }
        public string Image {  get; set; }
        public int Location_x { get; set; } = 0;
        public int Location_y { get; set; } = 0;
        public StatusAgent Status { get; set; } = StatusAgent.Dormant;

    }
}
