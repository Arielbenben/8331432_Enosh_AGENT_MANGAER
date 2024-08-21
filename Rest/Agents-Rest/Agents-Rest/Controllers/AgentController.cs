using Agents_Rest.Dto;
using Agents_Rest.Model;
using Agents_Rest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agents_Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController(IAgentService agentService) : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AgentModel>> GetByNickName(int id)
        {
            try
            {
                return Ok(await agentService.GetAgentByNickName(id));
            }
            catch
            {
                return BadRequest("Get request was wrong");
            }
        }



        [HttpPost("Post")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AgentIdDto>> Create([FromBody] AgentDto agentDto)
        {
            try
            {
                var agentId = await agentService.CreateNewAgent(agentDto);
                return Created("created successfully", agentId);
            }
            catch
            {
                return BadRequest("The create request was wrong");
            }
        }


        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await agentService.DeleteAgent(id);
                return Ok("Deleted successfully");
            }
            catch
            {
                return BadRequest("The delete request was wrong");
            }
        }


        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(int id, [FromBody] AgentDto agentDto)
        {
            try
            {
                await agentService.UpdateAgent(id, agentDto);
                return Ok("Updated");
            }
            catch
            {
                return BadRequest("The update request was wrong");
            }
        }

        [HttpPut("{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SetLocationAgent(int id, [FromBody] SetLocationAgentDto setLocationAgentDto)
        {
            try
            {
                await agentService.SetLocation(id, setLocationAgentDto);
                return Ok("updated location");
            }
            catch
            {
                return BadRequest("update location was wrong");
            }
        }


        [HttpPut("{id}/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateAgentLocation(int id, [FromBody] MoveLocationDto moveLocationDto)
        {

        }
    }
}
