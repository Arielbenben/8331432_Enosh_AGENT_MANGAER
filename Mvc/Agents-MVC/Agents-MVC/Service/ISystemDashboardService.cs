using Agents_MVC.ViewModel;

namespace Agents_MVC.Service
{
    public interface ISystemDashboardService
    {
        Task<GeneralDashboardVM> AddGeneralDashboardVM();
        Task<List<AgentsDetailsVM>> AddAgentsDetails();
        Task<List<TargetsDetailsVM>> AddTargetsDetails();
    }
}
