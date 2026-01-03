using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Spectrum.Models;
using Spectrum.Repositories;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ZXing;
using ZXing.Common;
using System.Drawing;
using SysDrawingImaging = System.Drawing.Imaging;
using Microsoft.AspNetCore.Hosting;

namespace Spectrum.Services;

public class EmailService
{
    private readonly EmailConfigRepository _configRepo;
    private readonly ILogger<EmailService> _logger;
    private readonly IUserRepository _userRepo;
    private readonly IWebHostEnvironment _env;

    public EmailService(EmailConfigRepository configRepo, ILogger<EmailService> logger, IUserRepository userRepo, IWebHostEnvironment env)
    {
        _configRepo = configRepo;
        _logger = logger;
        _userRepo = userRepo;
        _env = env;
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

    public async Task<bool> SendAppointmentEmailAsync(string toEmail, string? visitorName = null)
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
            message.Subject = "Create your visitor appointment";

            var displayName = string.IsNullOrWhiteSpace(visitorName) ? "Guest" : visitorName;
            var appointmentUrl = "https://192.168.1.54:6517/visitor-appointment";

            var htmlBody = $@"
                <html>
                  <body style=""font-family:Segoe UI, Arial, sans-serif; color:#222;"">
                    <p>Dear {System.Net.WebUtility.HtmlEncode(displayName)},</p>
                    <p>Thank you for your interest in visiting {System.Net.WebUtility.HtmlEncode(config.CompanyName ?? "our office")}. To schedule your visit, please click the button below and complete the appointment form:</p>
                    <p style=""text-align:center;"">
                      <a href=""{appointmentUrl}"" style=""display:inline-block;padding:10px 18px;background-color:#0078D4;color:#fff;text-decoration:none;border-radius:4px;"">Create your appointment</a>
                    </p>
                    <p>If the button above does not work, copy and paste the following URL into your browser:</p>
                    <p><a href=""{appointmentUrl}"">{appointmentUrl}</a></p>
                    <p>We look forward to welcoming you. If you have any questions, reply to this email.</p>
                    <p>Best regards,<br/>{System.Net.WebUtility.HtmlEncode(config.CompanyName ?? "JMN Infotech")}</p>
                  </body>
                </html>";

            message.Body = new TextPart("html") { Text = htmlBody };

            using var client = new SmtpClient();

            client.CheckCertificateRevocation = false;

            await client.ConnectAsync(config.SmtpHost, config.SmtpPort ?? 587, SecureSocketOptions.StartTls);
            if (!string.IsNullOrEmpty(config.EmailPassword))
            {
                await client.AuthenticateAsync(config.EmailAddress, config.EmailPassword);
            }

            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Appointment email sent successfully to {Email}", toEmail);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send appointment email to {Email}. Error: {Message}", toEmail, ex.Message);
            return false;
        }
    }

    public async Task<bool> SendGatepassEmailAsync(Visitor visitor, VisitorEntry entry, string? personToMeetName = null)
    {
        try
        {
            var config = await _configRepo.GetActiveConfigAsync();
            if (config == null)
            {
                _logger.LogWarning("No active email config found in database");
                return false;
            }

            if (string.IsNullOrWhiteSpace(visitor.Visitor_Email))
            {
                _logger.LogWarning("Visitor has no email. Skipping gatepass send for visitor id {VisitorId}", visitor.VisitorId);
                return false;
            }

            var toEmail = visitor.Visitor_Email!;

            // Fetch person to meet name if VisitorEntry_Userid is set
            if (string.IsNullOrWhiteSpace(personToMeetName) && entry.VisitorEntry_Userid.HasValue)
            {
                var user = await _userRepo.GetByIdAsync(entry.VisitorEntry_Userid.Value);
                personToMeetName = user?.U_Name ?? "N/A";
            }
            else
            {
                personToMeetName ??= "N/A";
            }

            var gatepassNo = entry.VisitorEntry_Gatepass ?? "N/A";
            var visitorName = string.IsNullOrWhiteSpace(visitor.Visitor_Name) ? "Guest" : visitor.Visitor_Name;
            var mobile = string.IsNullOrWhiteSpace(visitor.Visitor_mobile) ? "N/A" : visitor.Visitor_mobile;
            var vehicleNo = string.IsNullOrWhiteSpace(entry.VisitorEntry_Vehicleno) ? "N/A" : entry.VisitorEntry_Vehicleno;
            var purpose = string.IsNullOrWhiteSpace(entry.VisitorEntry_Purposeofvisit) ? "N/A" : entry.VisitorEntry_Purposeofvisit;
            var gpDate = (entry.CreatedDate == default) ? DateTime.Now : entry.CreatedDate;
            var gpDateStr = gpDate.ToString("M/d/yyyy");

            // Generate QR code for gatepass number
            var qrCodeBytes = GenerateQrCode(gatepassNo);

            // Generate PDF
            var pdfBytes = GenerateGatepassPdf(visitorName, gatepassNo, mobile, personToMeetName, vehicleNo, purpose, gpDateStr, visitor.Visitor_image, qrCodeBytes);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(config.CompanyName ?? "NoReply", config.EmailAddress));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "Your Digital Gate Pass";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
<html>
  <body style='font-family:Segoe UI, Arial, sans-serif; color:#222;'>
    <p>Dear {System.Net.WebUtility.HtmlEncode(visitorName)},</p>
    <p>Your visitor gate pass has been approved. Please find the attached PDF gate pass.</p>
    <p>Gate Pass No: <strong>{System.Net.WebUtility.HtmlEncode(gatepassNo)}</strong></p>
    <p>Date/Time: {System.Net.WebUtility.HtmlEncode(gpDateStr)}</p>
    <p>Please present this gate pass at the security gate.</p>
    <p>Best regards,<br/>{System.Net.WebUtility.HtmlEncode(config.CompanyName ?? "JMN Infotech")}</p>
  </body>
</html>"
            };

            bodyBuilder.Attachments.Add($"GatePass_{gatepassNo}.pdf", pdfBytes, new ContentType("application", "pdf"));
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            client.CheckCertificateRevocation = false;
            await client.ConnectAsync(config.SmtpHost, config.SmtpPort ?? 587, SecureSocketOptions.StartTls);
            if (!string.IsNullOrEmpty(config.EmailPassword))
            {
                await client.AuthenticateAsync(config.EmailAddress, config.EmailPassword);
            }

            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Gatepass PDF email sent successfully to {Email} for gatepass {GP}", toEmail, gatepassNo);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send gatepass PDF email. Error: {Message}", ex.Message);
            return false;
        }
    }

    // Replace barcode generation with QR code
    private byte[] GenerateQrCode(string text)
    {
        try
        {
            var qrWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 300,
                    Width = 300,
                    Margin = 1
                }
            };

            var pixelData = qrWriter.Write(text);
            using var bitmap = new Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            try
            {
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            using var ms = new MemoryStream();
            bitmap.Save(ms, SysDrawingImaging.ImageFormat.Png);
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "QR generation failed for {Text}", text);
            return Array.Empty<byte>();
        }
    }

    private byte[] GenerateGatepassPdf(string visitorName, string gatepassNo, string mobile, string personToMeet, string vehicleNo, string purpose, string gpDateTime, string? visitorImagePath, byte[] qrBytes)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        // helper conversions
        static float MmToPoints(float mm) => mm * 72f / 25.4f;
        static int MmToPixels(float mm, int dpi = 300) => (int)Math.Round(mm / 25.4f * dpi);

        // Visiting card size: 85 x 55 mm
        var pageWidthMm = 85f;
        var pageHeightMm = 55f;

        var widthPt = MmToPoints(pageWidthMm);
        var heightPt = MmToPoints(pageHeightMm);

        // Resolve visitor image physical path if relative
        string? visitorImagePhysical = null;
        if (!string.IsNullOrWhiteSpace(visitorImagePath))
        {
            if (Uri.IsWellFormedUriString(visitorImagePath, UriKind.Absolute))
            {
                visitorImagePhysical = null;
            }
            else
            {
                var trimmed = visitorImagePath.TrimStart('~').TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                var webRoot = _env.WebRootPath ?? _env.ContentRootPath;
                var full = Path.Combine(webRoot ?? string.Empty, trimmed);
                if (File.Exists(full)) visitorImagePhysical = full;
            }
        }

        // Photo size on card (mm) - further reduced
        var photoWidthMm = 18f;
        var photoHeightMm = 24f;

        byte[]? passportImageBytes = null;
        if (!string.IsNullOrWhiteSpace(visitorImagePhysical))
        {
            var pxW = MmToPixels(photoWidthMm, 300);
            var pxH = MmToPixels(photoHeightMm, 300);
            passportImageBytes = PreparePassportImage(visitorImagePhysical, targetWidth: pxW, targetHeight: pxH);
        }

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(new PageSize(widthPt, heightPt));
                // Minimal margins
                page.Margin(MmToPoints(1.5f));
                // Smaller default text
                page.DefaultTextStyle(x => x.FontSize(6.5f));

                page.Content().Element(content =>
                {
                    content.Container().Border(1).Padding(0).Column(mainCol =>
                    {
                        // Header - very compact
                        mainCol.Item().Background(Colors.Grey.Lighten3).PaddingVertical(3).PaddingHorizontal(3)
                            .AlignCenter().Text("Visitor Gate Pass").FontSize(9).SemiBold();

                        // Main content area
                        mainCol.Item().PaddingTop(3).PaddingHorizontal(3).PaddingBottom(2).Row(mainRow =>
                        {
                            // Left details - use Table for tighter layout
                            mainRow.RelativeColumn().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(MmToPoints(17));  // Label column
                                    columns.ConstantColumn(MmToPoints(2));   // Colon column
                                    columns.RelativeColumn();                // Value column
                                });

                                void AddRow(string label, string value)
                                {
                                    table.Cell().PaddingVertical(0.5f).Text(label).FontColor(Colors.Blue.Darken2).SemiBold().FontSize(6.5f);
                                    table.Cell().PaddingVertical(0.5f).Text(":").FontSize(6.5f);
                                    table.Cell().PaddingVertical(0.5f).Text(value).FontSize(6.5f);
                                }

                                AddRow("Visitor Name", visitorName);
                                AddRow("Gate Pass No", gatepassNo);
                                AddRow("Mobile No", mobile);
                                AddRow("Person To Meet", personToMeet);
                                AddRow("Vehicle No", vehicleNo);
                                AddRow("Purpose", purpose);
                                AddRow("GP Date/Time", gpDateTime);
                            });

                            // Right photo column - compact
                            mainRow.ConstantColumn(MmToPoints(photoWidthMm + 3)).AlignCenter().Element(photo =>
                            {
                                if (passportImageBytes != null && passportImageBytes.Length > 0)
                                {
                                    photo.Container().Padding(1).Border(1).Height(MmToPoints(photoHeightMm)).Width(MmToPoints(photoWidthMm)).Image(passportImageBytes).FitArea();
                                }
                                else if (!string.IsNullOrWhiteSpace(visitorImagePhysical))
                                {
                                    photo.Container().Padding(1).Border(1).Height(MmToPoints(photoHeightMm)).Width(MmToPoints(photoWidthMm)).Image(visitorImagePhysical).FitArea();
                                }
                                else
                                {
                                    photo.Container().Padding(1).Border(1).Height(MmToPoints(photoHeightMm)).Width(MmToPoints(photoWidthMm)).AlignCenter().Text("No Image").FontSize(5);
                                }
                            });
                        });

                        // Separator line
                        mainCol.Item().PaddingHorizontal(3).BorderBottom(0.5f);

                        // Footer area - compact
                        mainCol.Item().PaddingTop(2).PaddingHorizontal(3).PaddingBottom(3).Row(footerRow =>
                        {
                            // Signature area
                            footerRow.RelativeColumn().Column(sig =>
                            {
                                sig.Item().PaddingTop(20).AlignLeft().Text(" ").FontSize(6);
                                sig.Item().PaddingTop(9).Text("Operator Signature").FontSize(6);
                                sig.Item().Text("Security").FontSize(5.5f).FontColor(Colors.Grey.Darken2);
                            });

                            // QR area - smaller
                            var qrSizeMm = 14f;
                            footerRow.ConstantColumn(MmToPoints(qrSizeMm + 3)).AlignRight().Column(qc =>
                            {
                                qc.Item().AlignRight().Element(qe =>
                                {
                                    if (qrBytes.Length > 0)
                                    {
                                        qe.Container().Width(MmToPoints(qrSizeMm)).Height(MmToPoints(qrSizeMm)).Image(qrBytes).FitArea();
                                    }
                                });

                                qc.Item().AlignRight().Text("Scan QR").FontSize(5.5f).FontColor(Colors.Grey.Darken2).AlignRight();
                            });
                        });
                    });
                });
            });
        });

        using var ms = new MemoryStream();
        document.GeneratePdf(ms);
        return ms.ToArray();
    }

    private byte[]? PreparePassportImage(string imagePath, int targetWidth = 120, int targetHeight = 160)
    {
        try
        {
            if (!File.Exists(imagePath)) return null;

            using var src = System.Drawing.Image.FromFile(imagePath);
            var srcWidth = src.Width;
            var srcHeight = src.Height;

            // Calculate scale to cover the target box (crop center)
            var scale = Math.Max((double)targetWidth / srcWidth, (double)targetHeight / srcHeight);
            var scaledWidth = (int)Math.Ceiling(srcWidth * scale);
            var scaledHeight = (int)Math.Ceiling(srcHeight * scale);

            using var bitmap = new System.Drawing.Bitmap(targetWidth, targetHeight);
            using var g = System.Drawing.Graphics.FromImage(bitmap);
            g.Clear(System.Drawing.Color.White);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            // Draw scaled image centered (crop overflow)
            var destX = (targetWidth - scaledWidth) / 2;
            var destY = (targetHeight - scaledHeight) / 2;
            g.DrawImage(src, destX, destY, scaledWidth, scaledHeight);

            using var ms = new System.IO.MemoryStream();
            bitmap.Save(ms, SysDrawingImaging.ImageFormat.Png);
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to prepare passport image: {Path}", imagePath);
            return null;
        }
    }
}
