using Agents_Rest.Model;
using Agents_Rest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agents_Rest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MissionsController(IMissionService missionService) : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MissionModel>> GetAllMissions()
        {
            try
            {
                return Ok(await missionService.GetAllMissionsAsync());
            }
            catch
            {
                return BadRequest("Get request was wrong");
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateMissionToAssigned(MissionModel mission, AgentModel agent)
        {
            try
            {
                await missionService.UpdateMissionAssigned(mission, agent);
                return Ok("Updated");
            }
            catch
            {
                return BadRequest("Put request was wrong");
            }
        }
    }
}
