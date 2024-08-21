using Agents_Rest.Data;
using Agents_Rest.Model;
using Agents_Rest.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Agents_Rest.Service
{
    public class AgentService(ApplicationDbContext context) : IAgentService
    {

        public async Task<AgentModel> GetAgentByNickName(int id)
        {
            var agent = await context.Agents.FirstOrDefaultAsync(a => a.Id == id);
            if (agent == null) throw new Exception("There agent is not exists");

            return agent;
        }

        public async Task<AgentIdDto> CreateNewAgent(AgentDto agentDto)
        {
            AgentModel newAgent = new()
            {
                NickName = agentDto.NickName,
                Image = agentDto.Image,
            };

            await context.Agents.AddAsync(newAgent);
            await context.SaveChangesAsync();

            AgentIdDto agentIdDto = new () { Id = newAgent.Id };

            return agentIdDto;
        }

        public async Task DeleteAgent(int id)
        {
            var agent = await context.Agents.FirstOrDefaultAsync(a => a.Id == id);
            if (agent == null) throw new Exception("The agent is not exists");

            context.Agents.Remove(agent);
            await context.SaveChangesAsync();
            return;
        }

        public async Task UpdateAgent(int id, AgentDto agentDto)
        {
            var agent = await context.Agents.FirstOrDefaultAsync(a => a.Id == id);
            if (agent == null) throw new Exception("The agent is not exists");

            agent.NickName = agentDto.NickName;
            agent.Image = agentDto.Image;

            await context.SaveChangesAsync();
            return;
        }

        public async Task SetLocation(int id, SetLocationAgentDto setLocationAgentDto)
        {
            var agent = await context.Agents.FirstOrDefaultAsync(a => a.Id == id);
            if (agent == null) throw new Exception("The agent is not exists");

            agent.Location_x = setLocationAgentDto.X;
            agent.Location_y = setLocationAgentDto.Y;

            await context.SaveChangesAsync();
            return;
        }
    }
}
