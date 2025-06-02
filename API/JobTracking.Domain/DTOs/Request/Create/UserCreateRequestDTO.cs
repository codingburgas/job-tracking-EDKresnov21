using System.ComponentModel.DataAnnotations;
using JobTracking.Domain.Enums;

namespace JobTracking.Domain.DTOs.Request.Create;

public class UserCreateRequestDTO
{
    [Required, StringLength(64)]
    public string FirstName { get; set; }

    [Required, StringLength(64)]
    public string MiddleName { get; set; }

    [Required, StringLength(64)]
    public string LastName { get; set; }

    [Required, StringLength(32)]
    public string Username { get; set; }

    [Required, StringLength(128)]
    public string Password { get; set; }

    [Required]
    public UserRoleEnum Role { get; set; }
}