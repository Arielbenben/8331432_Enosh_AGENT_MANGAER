using static Agents_Rest.Utills.CalculateDistanceUtill;
using Agents_Rest.Data;
using Agents_Rest.Model;
using Microsoft.EntityFrameworkCore;

namespace Agents_Rest.Service
{
    public class MissionService(ApplicationDbContext context, IAgentService agentService) : IMissionService
    {
        public async Task<List<MissionModel>> GetAllMissionsAsync()
        {
            var missions = await context.Missions.ToListAsync();
            if (missions == null) throw new Exception("The is not missions");

            return missions;
        }

        public async Task CreateMission(AgentModel agent, TargetModel target)
        {
            MissionModel mission = new()
            {
                Agent = agent,
                AgentId = agent.Id,
                Target = target,
                TargetId = target.Id
                // time left = 
            };

            await context.Missions.AddAsync(mission);
            await context.SaveChangesAsync();

            return;
        }

        public double CalculateTimeLeft(AgentModel agent, TargetModel target)
        {
            var distance = CalculateDistance(agent, target);
            return distance / 5;
        }

        public async Task UpdateMissionAgentLocation(MissionModel mission)
        {
            await agentService.UpdateAgentLocationKillMission(mission.Agent, mission.Target);
            return;
        }

        public async Task UpdateMissionAssigned(MissionModel mission, AgentModel agent)
        {
            mission.Status = StatusMission.Assigned;
            mission.Agent = agent;

            await context.SaveChangesAsync();
            return;
        }
    }

    
}
