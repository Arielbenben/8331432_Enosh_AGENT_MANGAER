using Agents_Rest.Model;

namespace Agents_Rest.Service
{
    public interface IMissionService
    {
        Task<List<MissionModel>> GetAllMissionsAsync();

    }
}
