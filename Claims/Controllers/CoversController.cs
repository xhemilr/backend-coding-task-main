using Claims.Application.Exceptions;
using Claims.Application.Requests.Cover;
using Claims.Application.Responses;
using Claims.Application.Services;
using Claims.Core.Entities;
using Claims.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CoversController : ControllerBase
{
    private readonly ICoverService _coverService;

    public CoversController(ICoverService coverService)
    {
        _coverService = coverService;
    }

    [HttpPost]
    public async Task<ActionResult> ComputePremiumAsync(ComputePremiumRequest request)
    {
        var result = await _coverService.ComputePremiumAsync(request.StartDate, request.EndDate, request.CoverType);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CoverResponse>>> GetAllAsync()
    {
        var result = await _coverService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cover>> GetAsync(string id)
    {
        var result = await _coverService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateAsync(CreateCoverRequest cover)
    {
        try
        {
            var result = await _coverService.CreateAsync(cover);
            return Ok(result);
        }
        catch(InvalidDateException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        try
        {
            await _coverService.DeleteAsync(id);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}