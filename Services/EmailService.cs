using System.Net.Mail;
using System.Net;
using FileZipperWorkerService.Configurations;
using FileZipperWorkerService.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace FileZipperWorkerService.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailConfigurations _emailConfigurations;

        public EmailService(ILogger<EmailService> logger , IOptions<EmailConfigurations> emailConfigurations)
        {
            _logger = logger;
            _emailConfigurations = emailConfigurations.Value;
        }

        public async Task SendEmailWithAttachmentAsync(string zipFilePath , CancellationToken cancellationToken)
        {
            try
            {
                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailConfigurations.Email, "File Zipper Service"),
                    Subject = "Zipped Files from Target Folder",
                    Body = "Attached are the zipped files from the specified folder.",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(new MailAddress(_emailConfigurations.EmailRecipient));
                mailMessage.Attachments.Add(new Attachment(zipFilePath));

                using var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(_emailConfigurations.Email, _emailConfigurations.Password),
                    EnableSsl = true
                };

                _logger.LogInformation("Sending email to {recipient}", _emailConfigurations.EmailRecipient);
                await smtpClient.SendMailAsync(mailMessage , cancellationToken);
                _logger.LogInformation("Email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending the email.");
                throw;
            }
        }
    }
}
