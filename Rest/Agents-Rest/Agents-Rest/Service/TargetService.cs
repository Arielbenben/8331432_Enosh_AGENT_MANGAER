using Agents_Rest.Data;
using Agents_Rest.Dto;
using Agents_Rest.Model;
using static Agents_Rest.Utills.CalculateDistanceUtill;
using Microsoft.EntityFrameworkCore;

namespace Agents_Rest.Service
{
    public class TargetService(IServiceProvider serviceProvider) : ITargetService
    {
        private IMissionService missionService => serviceProvider.GetRequiredService<IMissionService>();


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
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            TargetModel newTarget = new()
            {
                Name = targetDto.name,
                Position = targetDto.position,
                ImageUrl = targetDto.PhotoUrl
            };

            await _context.Targets.AddAsync(newTarget);
            await _context.SaveChangesAsync();


            TargetIdDto targetIdDto = new () { Id = newTarget.Id };

            return targetIdDto;
        }

        public async Task DeleteTargetByIdAsync(int id)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var target = await _context.Targets.FirstOrDefaultAsync(x => x.Id == id);
            if (target == null) throw new Exception("Delete request was wrong");

            _context.Targets.Remove(target);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task<TargetModel> GetTargetByIdAsync(int id)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var target = await _context.Targets.FirstOrDefaultAsync(t => t.Id == id);
            if (target == null) throw new Exception("The target is not exists");

            return target;
        }
        public async Task<List<TargetModel>> GetAllTargetsAsync()
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var targets = await _context.Targets.ToListAsync();
            if (targets == null) throw new Exception("There is not targets");

            return targets;
        }

        public async Task UpdateTargetByIdAsync(int id, TargetDto targetDto)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var target = await _context.Targets.FirstOrDefaultAsync(t => t.Id == id);
            if (target == null) throw new Exception("The target is not exists");

            target.Name = targetDto.name;
            target.Position = targetDto.position;
            target.ImageUrl = targetDto.PhotoUrl;

            await _context.SaveChangesAsync();

            var potencialMissions = await CheckPosibilityMissionToTarget(target);  // need to send back

            return;
        }

        public async Task SetLocation(int id, SetLocationDto setLocationAgentDto)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var target = await _context.Targets.FirstOrDefaultAsync(_context => _context.Id == id) ??
                throw new Exception("The target not exists");

            target.LocationX = setLocationAgentDto.X;
            target.LocationY = setLocationAgentDto.Y;

            await _context.SaveChangesAsync();

            var potencialMissions = await CheckPosibilityMissionToTarget(target);  // need to send back

            return;
        }

        public async Task MoveLocation(int id, MoveLocationDto moveLocationDto)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var (x, y) = directions[moveLocationDto.direction];

            var target = await _context.Targets.FirstOrDefaultAsync(t => t.Id == id);
            if (target == null) throw new Exception("The target not exists");

            if (CheckLocationInRange(target, (x, y)))
            {
                target.LocationX += x;
                target.LocationY += y;
            }
            else
            {
                throw new Exception($"The location is out of range," +
                    $" current location: {(target.LocationX, target.LocationY)}");
            }

            await _context.SaveChangesAsync();

            var potencialMissions = await CheckPosibilityMissionToTarget(target);  // need to send back
            return;
        }

        public bool CheckLocationInRange(TargetModel target, (int x, int y) location)
        {
            return target.LocationX + location.x >= 0 && target.LocationX + location.x <= 1000
                && target.LocationY + location.y >= 0 && target.LocationY + location.y <= 1000;
        }

        public async Task<bool> TargetIsvalid(TargetModel target) // check
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            return !await _context.Missions.Where(m => m.Target == target)
                .AnyAsync(m => m.Status == StatusMission.Assigned);
        }

        public async Task<List<MissionModel>> CheckPosibilityMissionToTarget(TargetModel target)
        {
            var _context = DbContextFactory.CreateDbContext(serviceProvider);

            var dormantAgents = await _context.Agents.Where(a => a.Status == StatusAgent.Dormant).ToListAsync();
            var potentialAgents = dormantAgents.Where(a => CalculateDistance(a, target) <= 200).ToList();

            potentialAgents.ForEach(async p => await missionService.CreateMission(p, target));

            var potencialMission = await _context.Missions.Where(m => m.Target == target)
                .Where(m => m.Status == StatusMission.Offer)
                .Include(m => m.Target).Include(m => m.Agent).ToListAsync();

            return potencialMission;
        }

    }
}
