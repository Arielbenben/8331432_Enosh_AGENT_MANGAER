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

        // dictionary for the locations, make is easy to get location
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
                ImageUrl = agentDto.PhotoUrl,
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
            agent.ImageUrl = agentDto.PhotoUrl;

            await _context.SaveChangesAsync();

            // check if there are missions can be instruct to agent
            await CheckPosibilityMissionToAgent(agent);

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

            // check if there are missions can be instruct to agent
            await CheckPosibilityMissionToAgent(agent); 

            return;
        }

        public async Task MoveLocation(int id, MoveLocationDto moveLocationDto)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            //take the numbers of locations from the dictionary 
            var (x, y) = directions[moveLocationDto.direction];

            var agent = await _context.Agents.FirstOrDefaultAsync(a => a.Id == id);
            if (agent == null) throw new Exception("The agent not exists");
            // check if agent is valid
            if (agent.Status == StatusAgent.Active) throw new Exception(
                "The agent is active, It is not possible to change location");

            object _lock = new object();
            lock (_lock)
            {
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
            }

            await _context.SaveChangesAsync();

            // check if there are missions can be instruct to agent
            await CheckPosibilityMissionToAgent(agent);

            return;
        }

        public bool CheckLocationInRange(AgentModel agent, (int x, int y) location)
        {
            return agent.LocationX + location.x >= 0 && agent.LocationX + location.x <= 1000
                && agent.LocationY + location.y >= 0 && agent.LocationY + location.y <= 1000;
        }

        // when agent instructed to mission, the function move the agent one step to kill the target
        public async Task UpdateAgentLocationKillMission(AgentModel agent, TargetModel target) // check 
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var agentDB = await _context.Agents.FirstOrDefaultAsync(a => a.Id == agent.Id);
            if (agentDB == null) throw new Exception("The agent is not exists");
            
            int agentX = agentDB.LocationX.CompareTo(target.LocationX) switch
            {
                -1 => agentDB.LocationX + 1,
                1 => agentDB.LocationX - 1,
                _ => agentDB.LocationX + 0
            };

            int agentY = agentDB.LocationY.CompareTo(target.LocationY) switch
            {
                -1 => agentDB.LocationY + 1,
                1 => agentDB.LocationY - 1,
                _ => agentDB.LocationY + 0
            }; 

            agentDB.LocationX = agentX;
            agentDB.LocationY = agentY;

            
            await _context.SaveChangesAsync();
            return;
        }

        // check if there are missions can be instruct to agent
        public async Task<List<MissionModel>> CheckPosibilityMissionToAgent(AgentModel agent)
        {
            try
            {
                var _context = DbContextFactory.CreateDbContext(serviceProvider);

                var livingTargets = await _context.Targets
                    .Where(t => t.Status == StatusTarget.Living).ToListAsync();

                var potentialTargets = livingTargets.Where(t => CalculateDistance(agent, t) <= 200).ToList();

                //create missoins offers
                potentialTargets.ForEach(async p => await missionService.CreateMission(agent, p));

                //get the missions offers of the agent
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

        // refresh all the agent's offers
        public async Task RefreshAllAgentsPosibilityMissions()
        {
            var agents = await GetAllAgentsAsync();
            foreach (var agent in agents)
            {
                await CheckPosibilityMissionToAgent(agent);
            }

            return;
        }

        // move one step all the agents against the targets, check if complete mission and update the time left
        public async Task UpdateAllAgentsKillMission()
        {
            var missions = await missionService.GetAllMissionsAsync();
            var missionAssigned = missions.Where(m => m.Status == StatusMission.Assigned).ToList();

            if (missionAssigned.Count > 0)
            {
                foreach (var mission in missionAssigned)
                {
                    await UpdateAgentLocationKillMission(mission.Agent, mission.Target);
                    await missionService.CheckIfCompleteMission(mission);
                    await missionService.CalculateTimeLeft(mission);
                }
            }
            await missionService.RefreshAllMissiomMap();
        }
    }
}
