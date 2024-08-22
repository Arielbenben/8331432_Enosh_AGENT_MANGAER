using static Agents_Rest.Utills.CalculateDistanceUtill;
using Agents_Rest.Data;
using Agents_Rest.Model;
using Microsoft.EntityFrameworkCore;

namespace Agents_Rest.Service
{
    public class MissionService(ApplicationDbContext context) : IMissionService
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



    }

    
}
