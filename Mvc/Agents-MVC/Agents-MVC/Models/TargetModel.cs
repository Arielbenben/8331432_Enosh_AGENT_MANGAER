namespace Agents_MVC.Models
{
    public enum StatusTarget
    {
        Living,
        Assigned,
        Killed
    }

    public class TargetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int Location_x { get; set; } = 0;
        public int Location_y { get; set; } = 0;
        public string Image_url { get; set; }
        public StatusTarget Status { get; set; } = StatusTarget.Living;

    }
}
