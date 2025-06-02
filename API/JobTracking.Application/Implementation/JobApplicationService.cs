using JobTracking.Application.Contracts.Base;
using JobTracking.DataAccess.Data.Models;
using JobTracking.Domain.DTOs.Request.Create;
using JobTracking.Domain.DTOs.Request.Update;
using JobTracking.Domain.DTOs.Response;
using JobTracking.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace JobTracking.Application.Implementation;

public class JobApplicationService : IJobApplicationService
{
    protected DependencyProvider Provider { get; set; }
    
    public JobApplicationService(DependencyProvider provider)
    {
        Provider = provider;
    }

    public async Task<List<JobApplication>> GetAllJobApplications(int page, int pageCount)
    {
        return await Provider.Db.JobApplications
            .Skip((page - 1) * pageCount)
            .Take(pageCount)
            .ToListAsync();
    }
    
    public Task<JobApplicationResponseDTO?> GetJobApplication(int jobApplicationId)
    {
        return Provider.Db.JobApplications
            .Where(j => j.Id == jobApplicationId)
            .Select(j => new JobApplicationResponseDTO
            {
                Id = j.Id,
                UserId = j.UserId,
                JobAdId = j.JobAdId,
                Status = j.Status
            })
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<JobApplicationResponseDTO>> GetFilteredJobApplications(ApplicationStatusEnum? status, int? userId, int? jobAdId)
    {
        var query = Provider.Db.JobApplications.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(j => j.Status == status);
        }

        if (userId.HasValue)
        {
            query = query.Where(j => j.UserId == userId);
        }

        if (jobAdId.HasValue)
        {
            query = query.Where(j => j.JobAdId == jobAdId);
        }

        return await query.Select(j => new JobApplicationResponseDTO
        {
            Id = j.Id,
            UserId = j.UserId,
            JobAdId = j.JobAdId,
            Status = j.Status
        }).ToListAsync();
    }

    public async Task<JobApplicationResponseDTO> CreateJobApplication(JobApplicationCreateRequestDTO dto)
    {
        var entity = new JobApplication
        {
            UserId = dto.UserId,
            JobAdId = dto.JobAdId,
            Status = dto.Status,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "system",
            IsActive = true
        };

        Provider.Db.JobApplications.Add(entity);
        await Provider.Db.SaveChangesAsync();

        return new JobApplicationResponseDTO
        {
            Id = entity.Id,
            UserId = entity.UserId,
            JobAdId = entity.JobAdId,
            Status = entity.Status
        };
    }

    public async Task<bool> UpdateJobApplication(JobApplicationUpdateRequestDTO dto)
    {
        var entity = await Provider.Db.JobApplications.FindAsync(dto.Id);
        if (entity is null)
        {
            return false;
        }

        entity.Status = dto.Status;
        entity.UpdatedOn = DateTime.UtcNow;
        entity.UpdatedBy = "system";

        await Provider.Db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteJobApplication(int id)
    {
        var entity = await Provider.Db.JobApplications.FindAsync(id);
        if (entity is null)
        {
            return false;
        }

        Provider.Db.JobApplications.Remove(entity);
        await Provider.Db.SaveChangesAsync();
        return true;
    }
}