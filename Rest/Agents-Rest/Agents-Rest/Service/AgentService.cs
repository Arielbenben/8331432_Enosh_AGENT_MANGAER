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
        private IMissionService missionService = serviceProvider.GetRequiredService<IMissionService>();
        private ITargetService targetService = serviceProvider.GetRequiredService<ITargetService>();

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
            var agent = await context.Agents.FirstOrDefaultAsync(a => a.Id == id);
            if (agent == null) throw new Exception("There agent is not exists");

            return agent;
        }

        public async Task<List<AgentModel>> GetAllAgentsAsync()
        {
            var agents = await context.Agents.ToListAsync();
            if (agents == null) throw new Exception("The is not agents");

            return agents;
        }


        public async Task<AgentIdDto> CreateNewAgent(AgentDto agentDto)
        {
            AgentModel newAgent = new()
            {
                NickName = agentDto.NickName,
                Image_url = agentDto.PhotoUrl,
            };

            await context.Agents.AddAsync(newAgent);
            await context.SaveChangesAsync();

            await CheckPosibilityMissionToAgent(newAgent); // need to check where it go

            AgentIdDto agentIdDto = new() { Id = newAgent.Id };

            return agentIdDto;
        }

        public async Task DeleteAgent(int id)
        {
            var agent = await GetAgentById(id);

            context.Agents.Remove(agent);
            await context.SaveChangesAsync();
            return;
        }

        public async Task UpdateAgent(int id, AgentDto agentDto)
        {
            var agent = await GetAgentById(id);

            agent.NickName = agentDto.NickName;
            agent.Image_url = agentDto.PhotoUrl;

            await context.SaveChangesAsync();
            return;
        }

        public async Task SetLocation(int id, SetLocationDto setLocationAgentDto)
        {
            var agent = await GetAgentById(id);
            
            agent.Location_x = setLocationAgentDto.X;
            agent.Location_y = setLocationAgentDto.Y;

            await context.SaveChangesAsync();
            return;
        }

        public async Task MoveLocation(int id, MoveLocationDto moveLocationDto)
        {
            var (x, y) = directions[moveLocationDto.Location];

            var agent = await GetAgentById(id);
            // check if agent is valid
            if (agent.Status == StatusAgent.Active) throw new Exception(
                "The agent is active, It is not possible to change location"); 


            if (CheckLocationInRange(agent, (x, y)))
            {
                agent.Location_x += x;
                agent.Location_y += y;
            }
            else
            {
                throw new Exception($"The location is out of range," +
                    $" current location: {(agent.Location_x, agent.Location_y)}");
            }
        }

        public bool CheckLocationInRange(AgentModel agent, (int x, int y) location)
        {
            return agent.Location_x + location.x >= 0 && agent.Location_x + location.x <= 1000
                && agent.Location_y + location.y >= 0 && agent.Location_y + location.y <= 1000;
        }

        public async Task UpdateAgentLocationKillMission(AgentModel agent, TargetModel target) // neet to check
        {
            if(agent.Location_x != target.Location_x && agent.Location_y != target.Location_y)
            {
                if (agent.Location_x < target.Location_x)
                    agent.Location_x += 1;
                if (agent.Location_x > target.Location_x)
                    agent.Location_x -= 1;
                if (agent.Location_y < target.Location_y)
                    agent.Location_y += 1;
                if (agent.Location_y > target.Location_y)
                    agent.Location_y -= 1;
                else throw new Exception("The location is inValid");

                await context.SaveChangesAsync();
                return;
            }
            else
            {
                if(agent.Location_x != target.Location_x)
                {
                    if(agent.Location_x > target.Location_x)
                        agent.Location_x -= 1;
                    else
                        agent.Location_x += 1;

                    if (agent.Location_y < target.Location_y)
                        agent.Location_y -= 1;
                    else
                        agent.Location_y += 1;
                }

                await context.SaveChangesAsync();
                return;
            }
        }

        public async Task<List<MissionModel>> CheckPosibilityMissionToAgent(AgentModel agent)
        {
            var potentialTargets = await context.Targets.Where(t => t.StatusTarget == StatusTarget.Living)
                .Where(x => CalculateDistance(agent, x) <= 200).ToListAsync();

            foreach (var target in potentialTargets) {
                if (!await targetService.TargetIsvalid(target))
                {
                    potentialTargets.Remove(target);
                }
            }
            potentialTargets.Select(async p => await missionService.CreateMission(agent, p));

            var potencialMission = await context.Missions.Where(m => m.Agent == agent)
                .Where(m => m.Status == StatusMission.Offer).ToListAsync();

            return potencialMission;
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

            missions.ForEach(async m => await UpdateAgentLocationKillMission(m.Agent, m.Target));
        }
    }
}
