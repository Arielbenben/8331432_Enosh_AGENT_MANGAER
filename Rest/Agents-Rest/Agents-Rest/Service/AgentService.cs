using Agents_Rest.Data;
using Agents_Rest.Model;
using Agents_Rest.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Agents_Rest.Service
{
    public class AgentService(ApplicationDbContext context) : IAgentService
    {

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
                Image_url = agentDto.Photo_url,
            };

            await context.Agents.AddAsync(newAgent);
            await context.SaveChangesAsync();

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
            agent.Image_url = agentDto.Photo_url;

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

            if (CheckLocationInRange(agent, (x, y)))
            {
                agent.Location_x += x;
                agent.Location_y += y;
            }
            else
            {
                throw new Exception("The location is out of range");
            }
        }

        public bool CheckLocationInRange(AgentModel agent, (int x, int y) location)
        {
            return agent.Location_x + location.x >= 0 && agent.Location_x + location.x <= 1000
                && agent.Location_y + location.y >= 0 && agent.Location_y + location.y <= 1000;
        }
    }
}
