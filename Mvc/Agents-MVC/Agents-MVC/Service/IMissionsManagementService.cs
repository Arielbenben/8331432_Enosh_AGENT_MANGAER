using Agents_MVC.Models;

namespace Agents_MVC.Service
{
    public interface IMissionsManagementService
    {
        Task<Dictionary<AgentModel, List<MissionModel>>> GetAllOffers();
    }
}
