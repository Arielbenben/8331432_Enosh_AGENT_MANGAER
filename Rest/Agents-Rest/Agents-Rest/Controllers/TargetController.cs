using Agents_Rest.Data;
using Agents_Rest.Dto;
using Agents_Rest.Model;
using Agents_Rest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agents_Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetController(ApplicationDbContext context, ITargetService targetService) : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TargetModel>> GetAllTargets()
        {
            try
            {
                return Ok(await targetService.GetAllTargetsAsync());
            }
            catch
            {
                return BadRequest("Get request was wrong");
            }
        }


        [HttpGet("get/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TargetModel>> GetTargetById(int id)
        {
            try
            {
                return Ok(await targetService.GetTargetByIdAsync(id));
            }
            catch
            {
                return BadRequest("Get request was wrong");
            }
        }



        [HttpPost("Post")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TargetIdDto>> Create([FromBody] TargetDto targetDto)
        {
            try
            {
                var targetId = await targetService.CreateTarget(targetDto);
                return Created("created successfully", targetId);
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
                await targetService.DeleteTargetByIdAsync(id);
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
        public async Task<ActionResult> Update(int id, [FromBody] TargetDto targetDto)
        {
            try
            {
                await targetService.UpdateTargetByIdAsync(id, targetDto);
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
        public async Task<ActionResult> SetLocationTarget(int id, [FromBody] SetLocationDto setLocationDto)
        {
            try
            {
                await targetService.SetLocation(id, setLocationDto);
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
        public async Task<ActionResult> UpdateTargetLocation(int id, [FromBody] MoveLocationDto moveLocationDto)
        {
            try
            {
                await targetService.MoveLocation(id, moveLocationDto);
                return Ok("Location updated");
            }
            catch
            {
                return BadRequest("move location was wrong");
            }
        }
    }
}
