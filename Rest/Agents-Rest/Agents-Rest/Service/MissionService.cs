using static Agents_Rest.Utills.CalculateDistanceUtill;
using Agents_Rest.Data;
using Agents_Rest.Model;
using Microsoft.EntityFrameworkCore;

namespace Agents_Rest.Service
{
    public class MissionService(IServiceProvider serviceProvider) : IMissionService
    {

        private IAgentService agentService => serviceProvider.GetRequiredService<IAgentService>();

        public async Task<List<MissionModel>> GetAllMissionsAsync()
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var missions = await _context.Missions.Include(m => m.Target).Include(m => m.Agent).ToListAsync();
            if (missions == null) throw new Exception("The is not missions");

            return missions;
        }

        public async Task<MissionModel> GetMissionById(int id)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var mission = await _context.Missions.Include(m => m.Target)
                .Include(m => m.Agent).FirstOrDefaultAsync(m => m.Id == id);

            if (mission == null) throw new Exception("The mission is not exists");

            return mission;
        }

        public async Task<Dictionary<int, List<MissionModel>>> GetAllMissionsOffersToAgents()
        {
            var AllOffersToAgents = await agentService.RefreshAllAgentsPosibilityMissions();
            return AllOffersToAgents;
        }

        public async Task CreateMission(AgentModel agent, TargetModel target)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var checkMission = await _context.Missions.Where(m => m.AgentId == agent.Id)
                .FirstOrDefaultAsync(m => m.TargetId == target.Id);

            if (checkMission != null) return;

            MissionModel mission = new()
            {
                AgentId = agent.Id,
                TargetId = target.Id
            };

            await _context.Missions.AddAsync(mission);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task CalculateTimeLeft(MissionModel mission)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            double distance = CalculateDistance(mission.Agent, mission.Target);
            mission.TimeLeft = distance;

            await _context.SaveChangesAsync();
            return ;
        }

        public async Task UpdateMissionAgentLocation(MissionModel mission)
        {
            await agentService.UpdateAgentLocationKillMission(mission.Agent, mission.Target);
            return;
        }

        public async Task UpdateMissionAssigned(MissionModel mission)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            mission.Status = StatusMission.Assigned;
            mission.Agent.Status = StatusAgent.Active;
            mission.Target.Status = StatusTarget.Assigned;

            _context.Missions.RemoveRange(await _context.Missions
                .Where(m => m.Target == mission.Target)
                .Where(m => m.Status == StatusMission.Offer).ToListAsync());

            var potencialMisiions = await agentService.RefreshAllAgentsPosibilityMissions();

            await _context.SaveChangesAsync();
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

        public async Task CheckIfCompleteMission(List<MissionModel> MissionsAssigned)
        {
            foreach (var mission in MissionsAssigned)
            {
                if(mission.Target.LocationX == mission.Agent.LocationX && 
                    mission.Target.LocationY == mission.Agent.LocationY)
                {
                    mission.Agent.Status = StatusAgent.Dormant;
                    mission.Status = StatusMission.Eliminated;
                    await RefreshAllMissiomMap();
                }  
            }
            return;
        }

        public async Task<List<MissionModel>> RefreshAllMissiomMap()
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var allMissionLiving = await _context.Missions.Where(m => m.Status == StatusMission.Offer)
                .Include(m => m.Agent).Include(m => m.Target).ToListAsync();
            return allMissionLiving;
        }


    }
}
