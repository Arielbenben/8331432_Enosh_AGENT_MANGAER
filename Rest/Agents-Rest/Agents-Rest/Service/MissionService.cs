
using Agents_Rest.Data;
using Agents_Rest.Model;
using Microsoft.EntityFrameworkCore;

namespace Agents_Rest.Service
{
    public class MissionService(ApplicationDbContext context) : IMissionService
    {
        public async Task<List<MissionModel>> GetAllMissionsAsync()
        {
            var missions = await context.Missions.ToListAsync();
            if (missions == null) throw new Exception("The is not missions");

            return missions;
        }
    }
}
