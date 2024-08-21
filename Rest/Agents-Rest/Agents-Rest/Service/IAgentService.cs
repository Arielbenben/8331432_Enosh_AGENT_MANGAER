using Agents_Rest.Model;
using Agents_Rest.Dto;

namespace Agents_Rest.Service
{
    public interface IAgentService
    {
        Task<AgentModel> GetAgentByNickName(int id);
        Task<AgentIdDto> CreateNewAgent(AgentDto agentDto);
        Task UpdateAgent(int id, AgentDto agentDto);
        Task DeleteAgent(int id);
        Task SetLocation(int id, SetLocationAgentDto setLocationAgentDto);
    }
}
