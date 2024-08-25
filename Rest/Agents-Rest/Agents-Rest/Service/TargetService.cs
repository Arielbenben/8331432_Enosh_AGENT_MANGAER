using Agents_Rest.Data;
using Agents_Rest.Dto;
using Agents_Rest.Model;
using static Agents_Rest.Utills.CalculateDistanceUtill;
using Microsoft.EntityFrameworkCore;

namespace Agents_Rest.Service
{
    public class TargetService(ApplicationDbContext context, IServiceProvider serviceProvider) : ITargetService
    {

        private IMissionService missionService => serviceProvider.GetRequiredService<MissionService>();

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

        public async Task<TargetIdDto> CreateTarget(TargetDto targetDto)
        {
            TargetModel newTarget = new()
            {
                Name = targetDto.Name,
                Position = targetDto.Position,
                Image_url = targetDto.PhotoUrl
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
        public async Task<List<TargetModel>> GetAllTargetsAsync()
        {
            var targets = await context.Targets.ToListAsync();
            if (targets == null) throw new Exception("There is not targets");

            return targets;
        }

        public async Task UpdateTargetByIdAsync(int id, TargetDto targetDto)
        {
            var target = await context.Targets.FirstOrDefaultAsync(t => t.Id == id);
            if (target == null) throw new Exception("The target is not exists");

            target.Name = targetDto.Name;
            target.Position = targetDto.Position;
            target.Image_url = targetDto.PhotoUrl;

            await context.SaveChangesAsync();

            var potencialMissions = await CheckPosibilityMissionToTarget(target);  // need to send back

            return;
        }

        public async Task SetLocation(int id, SetLocationDto setLocationAgentDto)
        {
            var target = await GetTargetByIdAsync(id);

            target.Location_x = setLocationAgentDto.X;
            target.Location_y = setLocationAgentDto.Y;

            await context.SaveChangesAsync();

            var potencialMissions = await CheckPosibilityMissionToTarget(target);  // need to send back

            return;
        }

        public async Task MoveLocation(int id, MoveLocationDto moveLocationDto)
        {
            var (x, y) = directions[moveLocationDto.Location];

            var target = await GetTargetByIdAsync(id);

            if (CheckLocationInRange(target, (x, y)))
            {
                target.Location_x += x;
                target.Location_y += y;
            }
            else
            {
                throw new Exception($"The location is out of range," +
                    $" current location: {(target.Location_x, target.Location_y)}");
            }

            var potencialMissions = await CheckPosibilityMissionToTarget(target);  // need to send back
            return;
        }

        public bool CheckLocationInRange(TargetModel target, (int x, int y) location)
        {
            return target.Location_x + location.x >= 0 && target.Location_x + location.x <= 1000
                && target.Location_y + location.y >= 0 && target.Location_y + location.y <= 1000;
        }

        public async Task<bool> TargetIsvalid(TargetModel target) // check
        {
            return !await context.Missions.Where(m => m.Target == target)
                .AnyAsync(m => m.Status == StatusMission.Assigned);
        }

        public async Task<List<MissionModel>> CheckPosibilityMissionToTarget(TargetModel target)
        {
            var potentialAgents = await context.Agents.Where(a => a.Status == StatusAgent.Dormant)
                .Where(a => CalculateDistance(a, target) <= 200).ToListAsync();

            potentialAgents.Select(async p => await missionService.CreateMission(p, target));

            var potencialMission = await context.Missions.Where(m => m.Target == target)
                .Where(m => m.Status == StatusMission.Offer).ToListAsync();

            return potencialMission;
        }

    }
}
