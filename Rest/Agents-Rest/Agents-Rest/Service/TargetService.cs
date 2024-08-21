using Agents_Rest.Data;
using Agents_Rest.Dto;
using Agents_Rest.Model;
using Microsoft.EntityFrameworkCore;

namespace Agents_Rest.Service
{
    public class TargetService(ApplicationDbContext context) : ITargetService
    {
        public async Task<TargetIdDto> CreateTarget(TargetDto targetDto)
        {
            TargetModel newTarget = new()
            {
                Name = targetDto.Name,
                Position = targetDto.Position,
            };

            await context.Targets.AddAsync(newTarget);
            await context.SaveChangesAsync();

            TargetIdDto targetIdDto = new () { Id = newTarget.Id };

            return targetIdDto;
        }

        public async Task DeleteTargetByIdAsync(int id)
        {
            var target = await context.Targets.FirstOrDefaultAsync(x => x.Id == id);
            if (target == null) throw new Exception("Delete request was wrong");

            context.Targets.Remove(target);
            await context.SaveChangesAsync();
            return;
        }

        public async Task<TargetModel> GetTargetByIdAsync(int id)
        {
            var target = await context.Targets.FirstOrDefaultAsync(t => t.Id == id);
            if (target == null) throw new Exception("The target is not exists");

            return target;
        }

        public async Task UpdateTargetByIdAsync(int id, TargetDto targetDto)
        {
            var target = await context.Targets.FirstOrDefaultAsync(t => t.Id == id);
            if (target == null) throw new Exception("The target is not exists");

            target.Name = targetDto.Name;
            target.Position = targetDto.Position;

            await context.SaveChangesAsync();
            return;
        }
    }
}
