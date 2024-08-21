using Agents_Rest.Model;
using Agents_Rest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agents_Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionController(IMissionService missionService) : ControllerBase
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
    }
}
