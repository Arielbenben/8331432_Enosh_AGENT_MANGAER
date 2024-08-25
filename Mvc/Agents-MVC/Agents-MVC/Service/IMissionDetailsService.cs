using Agents_MVC.Models;

namespace Agents_MVC.Service
{
    public interface IMissionDetailsService
    {
        Task<MissionModel> GetMissionById(int id);
    }
}
