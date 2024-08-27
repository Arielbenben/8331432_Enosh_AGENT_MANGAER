using Agents_MVC.Models;
using Agents_MVC.ViewModel;

namespace Agents_MVC.Service
{
    public interface ISystemDashboardService
    {
        Task<GeneralDashboardVM> AddGeneralDashboardVM();
        Task<List<AgentsDetailsVM>> AddAgentsDetails();
        Task<List<TargetsDetailsVM>> AddTargetsDetails();
        Task<List<AgentModel>> GetAllAgents();
        Task<List<TargetModel>> GetAllTargets();
        Task<List<MissionModel>> GetAllMissions();
    }
}
