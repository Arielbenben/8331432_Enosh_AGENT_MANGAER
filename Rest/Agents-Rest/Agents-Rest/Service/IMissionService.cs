﻿using Agents_Rest.Model;

namespace Agents_Rest.Service
{
    public interface IMissionService
    {
        Task<List<MissionModel>> GetAllMissionsAsync();
        Task CreateMission(AgentModel agent, TargetModel target);
        Task CalculateTimeLeft(MissionModel mission);
        Task UpdateMissionAgentLocation(MissionModel mission);
        Task UpdateMissionAssigned(int id);
        Task UpdateMissiomMoveAgentsActive();
        Task UpdateAllMissionsTimeLeft();
        Task<List<MissionModel>> RefreshAllMissiomMap();
        Task<MissionModel> GetMissionById(int id);
        Task CheckIfCompleteMission(MissionModel MissionsAssigned);

    }
}
