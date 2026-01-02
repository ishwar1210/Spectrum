namespace Spectrum.Models;

public class EmailConfig
{
    public int CompanyEmailId { get; set; }
    public string? CompanyName { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
    public string EmailPassword { get; set; } = string.Empty; // stored encrypted/hashed in DB ideally
    public string? SmtpHost { get; set; }
    public int? SmtpPort { get; set; }
    public bool? EnableSSL { get; set; }
    public bool? IsActive { get; set; }
}
