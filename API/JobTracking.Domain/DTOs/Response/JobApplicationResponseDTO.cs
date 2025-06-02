using JobTracking.Domain.Enums;

namespace JobTracking.Domain.DTOs.Response;

public class JobApplicationResponseDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int JobAdId { get; set; }
    public ApplicationStatusEnum Status { get; set; }
}