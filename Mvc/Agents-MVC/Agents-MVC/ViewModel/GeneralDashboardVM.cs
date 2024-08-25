namespace Agents_MVC.ViewModel
{
    public class GeneralDashboardVM
    {
        public int SumAgents { get; set; }
        public int SumAgentsActive { get; set; }
        public int SumTargets { get; set; }
        public int SumTargetsKilled { get; set; }
        public int SumMissions { get; set; }
        public int SumMissionsAssigned { get; set; }
        public double CompareAgentsToTargets { get; set; }
        public double CompareAgentsDormantsToTargets { get; set; }
    }
}
