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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MissionModel>> GetMissionById(int id)
        {
            try
            {
                return Ok(await missionService.GetMissionById(id));
            }
            catch
            {
                return BadRequest("Get request was wrong");
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateMissionToAssigned(int id)
        {
            try
            {
                await missionService.UpdateMissionAssigned(id);
                return Ok("Updated");
            }
            catch
            {
                return BadRequest("Put request was wrong");
            }
        }

        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateAgentToKillAndTimeLeft()
        {
            try
            {
                await missionService.UpdateMissiomMoveAgentsActive();
                await missionService.UpdateAllMissionsTimeLeft();

                return Ok("Updated");
            }
            catch
            {
                return BadRequest("Put request was wrong");
            }
        }
    }
}
