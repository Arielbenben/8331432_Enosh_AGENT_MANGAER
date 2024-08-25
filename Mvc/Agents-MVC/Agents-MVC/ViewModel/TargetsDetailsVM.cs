using Agents_MVC.Models;

namespace Agents_MVC.ViewModel
{
    public class TargetsDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int LocationX { get; set; } 
        public int LocationY { get; set; } 
        public string ImageUrl { get; set; }
        public StatusTarget Status { get; set; }

    }
}
