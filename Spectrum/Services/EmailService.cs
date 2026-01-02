using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Spectrum.Models;
using Spectrum.Repositories;

namespace Spectrum.Services;

public class EmailService
{
    private readonly EmailConfigRepository _configRepo;
    private readonly ILogger<EmailService> _logger;

    public EmailService(EmailConfigRepository configRepo, ILogger<EmailService> logger)
    {
        _configRepo = configRepo;
        _logger = logger;
    }

    public async Task<bool> SendUserCredentialsEmailAsync(string toEmail, string userName, string plainPassword)
    {
        try
        {
            var config = await _configRepo.GetActiveConfigAsync();
            if (config == null)
            {
                _logger.LogWarning("No active email config found in database");
                return false;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(config.CompanyName ?? "NoReply", config.EmailAddress));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "Your account credentials - JMN Infotech";

            var body = $@"Hello,

Your account has been created successfully.

Username: {userName}
Password: {plainPassword}

Please change your password after first login for security.

Regards,
{config.CompanyName ?? "JMN Infotech"}";

            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            
            // Gmail requires StartTls on port 587
            await client.ConnectAsync(config.SmtpHost, config.SmtpPort ?? 587, SecureSocketOptions.StartTls);
            
            if (!string.IsNullOrEmpty(config.EmailPassword))
            {
                await client.AuthenticateAsync(config.EmailAddress, config.EmailPassword);
            }
            
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            
            _logger.LogInformation("Email sent successfully to {Email}", toEmail);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}. Error: {Message}", toEmail, ex.Message);
            return false;
        }
    }
}
