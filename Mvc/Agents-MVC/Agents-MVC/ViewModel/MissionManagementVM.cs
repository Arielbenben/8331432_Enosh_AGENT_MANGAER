using Agents_MVC.Models;

namespace Agents_MVC.ViewModel
{
    public class MissionManagementVM
    {
        public int Id { get; set; }
        public string AgentNickName { get; set; }
        public int AgentLocationX { get; set; }
        public int AgentLocationY { get; set; }
        public string TargetName { get; set; }
        public string TargetPosition { get; set; }
        public int TargetLocationX { get; set; }
        public int TargetLocationY { get; set; }
        public double Distance { get; set; }
        public double TimeLeft { get; set; }
    }
}
