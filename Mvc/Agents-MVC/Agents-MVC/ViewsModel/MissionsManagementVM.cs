namespace Agents_MVC.ViewsModel
{
    public class MissionsManagementVM
    {
        public string NickName { get; set; }
        public Tuple<string, string> AgentLocation {  get; set; }
        public string TargetName { get; set; }
        public Tuple<string, string> TargetLocation { get; set; }
        public string Notes { get; set; }
        public double Distance { get; set; }
        public string TimeToeLiminate { get; set; }
    }
}
