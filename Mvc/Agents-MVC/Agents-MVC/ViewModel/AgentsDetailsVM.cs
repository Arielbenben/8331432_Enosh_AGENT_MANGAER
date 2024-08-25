using Agents_MVC.Models;

namespace Agents_MVC.ViewModel
{
    public class AgentsDetailsVM
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string ImageUrl { get; set; }
        public int Location_x { get; set; }
        public int Location_y { get; set; }
        public StatusAgent Status { get; set; }
        public int MissionId { get; set; }
        public double TimeLeftToKill { get; set; }
        public int SumEliminates { get; set; }

    }
}
