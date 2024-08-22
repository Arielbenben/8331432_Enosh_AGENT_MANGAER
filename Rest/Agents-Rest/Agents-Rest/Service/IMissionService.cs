using Agents_Rest.Model;

namespace Agents_Rest.Service
{
    public interface IMissionService
    {
        Task<List<MissionModel>> GetAllMissionsAsync();
        Task CreateMission(AgentModel agent, TargetModel target);
        double CalculateTimeLeft(AgentModel agent, TargetModel target);
        Task UpdateMissionAgentLocation(MissionModel mission);
        Task UpdateMissionAssigned(MissionModel mission, AgentModel agent);

    }
}
