using Agents_Rest.Model;
using Agents_Rest.Dto;

namespace Agents_Rest.Service
{
    public interface ITargetService
    {
        Task<TargetModel> GetTargetByIdAsync(int id);
        Task<TargetIdDto> CreateTarget(TargetDto targetDto);
        Task UpdateTargetByIdAsync(int id, TargetDto targetDto);
        Task DeleteTargetByIdAsync(int id);
    }
}
