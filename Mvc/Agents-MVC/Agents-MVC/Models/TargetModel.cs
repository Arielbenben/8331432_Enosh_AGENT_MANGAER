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
        public int LocationX { get; set; } = 0;
        public int LocationY { get; set; } = 0;
        public string ImageUrl { get; set; }
        public StatusTarget Status { get; set; } = StatusTarget.Living;

    }
}
