using Claims.Application.Exceptions;
using Claims.Application.Requests.Claim;
using Claims.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimService _claimService;

        public ClaimsController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllAsync()
        {
            var result = await _claimService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateAsync(CreateClaimRequest claim)
        {
            try
            {
                var result = await _claimService.CreateAsync(claim);
                return Ok(result);
            }
            catch(EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidDateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            try
            {
                await _claimService.DeleteAsync(id);
                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAsync(string id)
        {
            var result = await _claimService.GetByIdAsync(id);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}