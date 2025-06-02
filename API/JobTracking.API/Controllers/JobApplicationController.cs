using JobTracking.Application.Contracts.Base;
using JobTracking.Domain.DTOs.Request.Create;
using JobTracking.Domain.DTOs.Request.Update;
using JobTracking.Domain.DTOs.Response;
using JobTracking.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace JobTracking.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class JobApplicationController : Controller
{
    private readonly IJobApplicationService _jobApplicationService;
    
    public JobApplicationController(IJobApplicationService jobApplicationService)
    {
        _jobApplicationService = jobApplicationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _jobApplicationService.GetJobApplication(id));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobApplicationResponseDTO>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageCount = 10)
    {
        var result = await _jobApplicationService.GetAllJobApplications(page, pageCount);
        return Ok(result);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<JobApplicationResponseDTO>>> GetFiltered(
        [FromQuery] ApplicationStatusEnum? status,
        [FromQuery] int? userId,
        [FromQuery] int? jobAdId)
    {
        var result = await _jobApplicationService.GetFilteredJobApplications(status, userId, jobAdId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<JobApplicationResponseDTO>> Add([FromBody] JobApplicationCreateRequestDTO dto)
    {
        var created = await _jobApplicationService.CreateJobApplication(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] JobApplicationUpdateRequestDTO dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("ID mismatch.");
        }

        var success = await _jobApplicationService.UpdateJobApplication(dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _jobApplicationService.DeleteJobApplication(id);
        return success ? NoContent() : NotFound();
    }
}