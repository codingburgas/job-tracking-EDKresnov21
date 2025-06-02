using JobTracking.DataAccess.Data.Models;
using JobTracking.Domain.DTOs.Request.Create;
using JobTracking.Domain.DTOs.Request.Update;
using JobTracking.Domain.DTOs.Response;

namespace JobTracking.Application.Contracts.Base;

public interface IJobAdService
{
    public Task<List<JobAd>> GetAllJobAds(int page, int pageCount);
    public Task<JobAdResponseDTO?> GetJobAd(int userId);
    public Task<JobAdResponseDTO> CreateJobAd(JobAdCreateRequestDTO dto);
    public Task<bool> UpdateJobAd(JobAdUpdateRequestDTO dto);
    public Task<bool> DeleteJobAd(int id);
    public Task<List<JobAdResponseDTO>> GetFilteredJobAds(string? title, bool? isOpen);
}