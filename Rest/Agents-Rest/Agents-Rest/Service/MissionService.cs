﻿using static Agents_Rest.Utills.CalculateDistanceUtill;
using Agents_Rest.Data;
using Agents_Rest.Model;
using Microsoft.EntityFrameworkCore;

namespace Agents_Rest.Service
{
    public class MissionService(ApplicationDbContext context, IServiceProvider serviceProvider) : IMissionService
    {

        private IAgentService agentService = serviceProvider.GetRequiredService<IAgentService>();

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

        public async Task CalculateTimeLeft(MissionModel mission)
        {
            double distance = CalculateDistance(mission.Agent, mission.Target);
            mission.TimeLeft = distance;

            await context.SaveChangesAsync();
            return;
        }

        public async Task UpdateMissionAgentLocation(MissionModel mission)
        {
            await agentService.UpdateAgentLocationKillMission(mission.Agent, mission.Target);
            return;
        }

        public async Task UpdateMissionAssigned(MissionModel mission)
        {
            mission.Status = StatusMission.Assigned;

            context.Missions.RemoveRange(await context.Missions
                .Where(m => m.Target == mission.Target)
                .Where(m => m.Status == StatusMission.Offer).ToListAsync());

            await agentService.RefreshAllAgentsPosibilityMissions();

            await context.SaveChangesAsync();
            return;
        }

        public async Task UpdateMissiomMoveAgentsActive()
        {
            await agentService.UpdateAllAgentsKillMission();
            return;
        }

        public async Task UpdateAllMissionsTimeLeft()
        {
            var missions = await GetAllMissionsAsync();
            var missionAssigned = missions.Where(m => m.Status == StatusMission.Assigned).ToList();

            missions.ForEach(async m => await CalculateTimeLeft(m));
            return;
        }


    }
}
