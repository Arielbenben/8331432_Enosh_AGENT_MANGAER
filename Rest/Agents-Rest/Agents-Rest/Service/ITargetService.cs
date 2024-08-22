using Agents_Rest.Model;
using Agents_Rest.Dto;

namespace Agents_Rest.Service
{
    public interface ITargetService
    {
        Task<TargetModel> GetTargetByIdAsync(int id);
        Task<List<TargetModel>> GetAllTargetsAsync();
        Task<TargetIdDto> CreateTarget(TargetDto targetDto);
        Task UpdateTargetByIdAsync(int id, TargetDto targetDto);
        Task DeleteTargetByIdAsync(int id);
        Task SetLocation(int id, SetLocationDto setLocationAgentDto);
        Task MoveLocation(int id, MoveLocationDto moveLocationDto);
        bool CheckLocationInRange(TargetModel target, (int x, int y) location);
        Task<bool> TargetIsvalid(TargetModel target);
        Task<List<MissionModel>> CheckPosibilityMissionToTarget(TargetModel target);
    }
}
