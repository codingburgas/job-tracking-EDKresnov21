using JobTracking.Application.Contracts.Base;
using JobTracking.Domain.DTOs.Request.Create;
using JobTracking.Domain.DTOs.Request.Update;
using JobTracking.Domain.DTOs.Response;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace JobTracking.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class JobAdController : Controller
{
    private readonly IJobAdService _jobAdService;
    
    public JobAdController(IJobAdService jobAdService)
    {
        _jobAdService = jobAdService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobAdResponseDTO>> GetById(int id)
    {
        var result = await _jobAdService.GetJobAd(id);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobAdResponseDTO>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageCount = 10)
    {
        var result = await _jobAdService.GetAllJobAds(page, pageCount);

        var response = result.Select(j => new JobAdResponseDTO
        {
            Id = j.Id,
            Title = j.Title,
            CompanyName = j.CompanyName,
            Description = j.Description,
            PublishedOn = j.PublishedOn,
            IsOpen = j.IsOpen
        });

        return Ok(response);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<JobAdResponseDTO>>> GetFiltered([FromQuery] string? title,
        [FromQuery] bool? isOpen)
    {
        var result = await _jobAdService.GetFilteredJobAds(title, isOpen);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<JobAdResponseDTO>> Add([FromBody] JobAdCreateRequestDTO dto)
    {
        var created = await _jobAdService.CreateJobAd(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] JobAdUpdateRequestDTO dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("ID mismatch.");
        }

        var success = await _jobAdService.UpdateJobAd(dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _jobAdService.DeleteJobAd(id);
        return success ? NoContent() : NotFound();
    }
}