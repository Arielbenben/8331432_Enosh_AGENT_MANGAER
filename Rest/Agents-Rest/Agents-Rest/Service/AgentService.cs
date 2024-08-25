using Agents_Rest.Data;
using Agents_Rest.Model;
using Agents_Rest.Dto;
using static Agents_Rest.Utills.CalculateDistanceUtill;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Agents_Rest.Service
{
    public class AgentService(ApplicationDbContext context, IServiceProvider serviceProvider) : IAgentService
    {
        private IMissionService missionService => serviceProvider.GetRequiredService<IMissionService>();
        private ITargetService targetService => serviceProvider.GetRequiredService<ITargetService>();

        private readonly Dictionary<string, (int x, int y)> directions = new()
        {
            {"n", (x: 0,y: 1) },
            {"s", (x: 0,y: -1) },
            {"e", (x: 1,y: 0) },
            {"w", (x: -1,y: 0) },
            {"nw", (x: -1,y: 1) },
            {"ne", (x: 1,y: 1) },
            {"sw", (x: -1,y: -1) },
            {"se", (x: 1,y: -1) }
        };

        public async Task<AgentModel> GetAgentById(int id)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var agent = await _context.Agents.FirstOrDefaultAsync(a => a.Id == id);
            if (agent == null) throw new Exception("There agent is not exists");

            return agent;
        }

        public async Task<List<AgentModel>> GetAllAgentsAsync()
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var agents = await _context.Agents.ToListAsync();
            if (agents == null) throw new Exception("The is not agents");

            return agents;
        }


        public async Task<AgentIdDto> CreateNewAgent(AgentDto agentDto)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            AgentModel newAgent = new()
            {
                NickName = agentDto.nickName,
                Image_url = agentDto.PhotoUrl,
            };

            await _context.Agents.AddAsync(newAgent);
            await _context.SaveChangesAsync();

            AgentIdDto agentIdDto = new() { Id = newAgent.Id };

            return agentIdDto;
        }

        public async Task DeleteAgent(int id)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var agent = await GetAgentById(id);

            context.Agents.Remove(agent);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task UpdateAgent(int id, AgentDto agentDto)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var agent = await GetAgentById(id);

            agent.NickName = agentDto.nickName;
            agent.Image_url = agentDto.PhotoUrl;

            await _context.SaveChangesAsync();

            var potencialMissions = await CheckPosibilityMissionToAgent(agent); // need to send back

            return;
        }

        public async Task SetLocation(int id, SetLocationDto setLocationAgentDto)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var agent = await _context.Agents.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new Exception("The agent not exists");
            
            agent.LocationX = setLocationAgentDto.X;
            agent.LocationY = setLocationAgentDto.Y;

            await _context.SaveChangesAsync();

            var potencialMissions = await CheckPosibilityMissionToAgent(agent); // need to check where it go

            return;
        }

        public async Task MoveLocation(int id, MoveLocationDto moveLocationDto)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var (x, y) = directions[moveLocationDto.Location];

            var agent = await GetAgentById(id);
            // check if agent is valid
            if (agent.Status == StatusAgent.Active) throw new Exception(
                "The agent is active, It is not possible to change location"); 


            if (CheckLocationInRange(agent, (x, y)))
            {
                agent.LocationX += x;
                agent.LocationY += y;
            }
            else
            {
                throw new Exception($"The location is out of range," +
                    $" current location: {(agent.LocationX, agent.LocationY)}");
            }

            await _context.SaveChangesAsync();

            var potencialMissions = await CheckPosibilityMissionToAgent(agent); // need to check where it go

            return;
        }

        public bool CheckLocationInRange(AgentModel agent, (int x, int y) location)
        {
            return agent.LocationX + location.x >= 0 && agent.LocationX + location.x <= 1000
                && agent.LocationY + location.y >= 0 && agent.LocationY + location.y <= 1000;
        }

        private int ComputePostion(int x, int y) => x.CompareTo(y) switch
        {
            -1 => 1,
            1 =>  - 1,
            _ => 0
        };

        public async Task UpdateAgentLocationKillMission(AgentModel agent, TargetModel target)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);
            
            int agentX = agent.LocationX + ComputePostion(agent.LocationX, target.LocationX);

            int agentY = agent.LocationY + ComputePostion(agent.LocationY, target.LocationY);

            agent.LocationX = agentX;
            agent.LocationY = agentY;   
           
            await _context.SaveChangesAsync();
            return;
        }

        public async Task<List<MissionModel>> CheckPosibilityMissionToAgent(AgentModel agent)
        {
            try
            {
                var _context = DbContextFactory.CreateDbContext(serviceProvider);

                var livingTargets = await _context.Targets
                    .Where(t => t.Status == StatusTarget.Living).ToListAsync();

                var potentialTargets = livingTargets.Where(t => CalculateDistance(agent, t) <= 200).ToList();

                potentialTargets.ForEach(async p => await missionService.CreateMission(agent, p));

                var potencialMission = await _context.Missions.Where(m => m.Agent == agent)
                    .Where(m => m.Status == StatusMission.Offer)
                    .Include(m => m.Agent).Include(m => m.Target).ToListAsync();

                return potencialMission;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<Dictionary<AgentModel, List<MissionModel>>> RefreshAllAgentsPosibilityMissions()
        {
            Dictionary<AgentModel, List<MissionModel>> agentsMissions = new();

            var agents = await GetAllAgentsAsync();
            foreach (var agent in agents)
            {
                agentsMissions[agent] = await CheckPosibilityMissionToAgent(agent);
            }

            return agentsMissions;
        }

        public async Task UpdateAllAgentsKillMission()
        {
            var missions = await missionService.GetAllMissionsAsync();
            var missionAssigned = missions.Where(m => m.Status == StatusMission.Assigned).ToList();

            if (missionAssigned.Count > 0)
            {
                missions.ForEach(async m => await UpdateAgentLocationKillMission(m.Agent, m.Target));
            }
            await missionService.RefreshAllMissiomMap();
        }
    }
}
