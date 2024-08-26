using System.ComponentModel.DataAnnotations;

namespace Agents_Rest.Model
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

        [Required]
        public string Name { get; set; }

        [Required]
        public string Position { get; set; }
        public int LocationX { get; set; } = 0;
        public int LocationY { get; set; } = 0;
        public string ImageUrl { get; set; }
        public StatusTarget Status { get; set; } = StatusTarget.Living;

    }
}
