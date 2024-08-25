using Agents_Rest.Model;

namespace Agents_Rest.Service
{
    public interface IMissionService
    {
        Task<List<MissionModel>> GetAllMissionsAsync();
        Task CreateMission(AgentModel agent, TargetModel target);
        Task CalculateTimeLeft(MissionModel mission);
        Task UpdateMissionAgentLocation(MissionModel mission);
        Task UpdateMissionAssigned(MissionModel mission);
        Task UpdateMissiomMoveAgentsActive();
        Task UpdateAllMissionsTimeLeft();
        Task<List<MissionModel>> RefreshAllMissiomMap();

    }
}
