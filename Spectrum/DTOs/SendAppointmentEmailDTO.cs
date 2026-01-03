using System.ComponentModel.DataAnnotations;

namespace Spectrum.DTOs;

public class SendAppointmentEmailDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    // Optional friendly name to use in the email greeting
    public string? Name { get; set; }
}