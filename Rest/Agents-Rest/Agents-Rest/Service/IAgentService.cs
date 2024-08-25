using Agents_Rest.Model;
using Agents_Rest.Dto;

namespace Agents_Rest.Service
{
    public interface IAgentService
    {
        Task<AgentModel> GetAgentById(int id);
        Task<List<AgentModel>> GetAllAgentsAsync();
        Task<AgentIdDto> CreateNewAgent(AgentDto agentDto);
        Task UpdateAgent(int id, AgentDto agentDto);
        Task DeleteAgent(int id);
        Task SetLocation(int id, SetLocationDto setLocationAgentDto);
        Task MoveLocation(int id, MoveLocationDto moveLocationDto);
        bool CheckLocationInRange(AgentModel agent, (int x, int y) location);
        Task UpdateAgentLocationKillMission(AgentModel agent, TargetModel target);
        Task<Dictionary<int, List<MissionModel>>> RefreshAllAgentsPosibilityMissions();
        Task UpdateAllAgentsKillMission();
    }
}
